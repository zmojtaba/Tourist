import json
import os
import pika
import redis

# ---------- Hardcoded config ----------
RABBITMQ_HOST = "localhost"
RABBITMQ_PORT = 5672
RABBITMQ_USER = "admin"
RABBITMQ_PASS = "admin"
RABBITMQ_VHOST = "/"
QUEUE_NAME = "frame.processing.yolo"

REDIS_HOST = "localhost"
REDIS_PORT = 6379
REDIS_DB = 0

OUTPUT_DIR = "./frames"
# ---------------------------------------


def main():
    os.makedirs(OUTPUT_DIR, exist_ok=True)

    # Redis client
    r = redis.Redis(host=REDIS_HOST, port=REDIS_PORT, db=REDIS_DB)

    # RabbitMQ connection
    credentials = pika.PlainCredentials(RABBITMQ_USER, RABBITMQ_PASS)
    params = pika.ConnectionParameters(
        host=RABBITMQ_HOST,
        port=RABBITMQ_PORT,
        virtual_host=RABBITMQ_VHOST,
        credentials=credentials,
    )
    connection = pika.BlockingConnection(params)
    channel = connection.channel()

    # Passive declare: only checks the queue exists, does NOT create or
    # modify it. This avoids PRECONDITION_FAILED when the broker already
    # has the queue with arguments like x-dead-letter-exchange.
    channel.queue_declare(queue=QUEUE_NAME, passive=True)

    # Only fetch one message at a time
    channel.basic_qos(prefetch_count=1)

    def on_message(ch, method, properties, body):
        try:
            signal = json.loads(body)
            print("Received signal:", signal)

            frame_id = signal["id"]
            camera_id = signal["cameraId"]
            blob_key = signal["blobKey"]

            jpeg_bytes = r.get(blob_key)
            if jpeg_bytes is None:
                print(f"Frame {blob_key} not found in Redis (expired?)")
                ch.basic_ack(delivery_tag=method.delivery_tag)
                return

            filename = os.path.join(OUTPUT_DIR, f"{camera_id}_{frame_id}.jpg")
            with open(filename, "wb") as f:
                f.write(jpeg_bytes)

            print(f"Saved frame to {filename} ({len(jpeg_bytes)} bytes)")

            ch.basic_ack(delivery_tag=method.delivery_tag)

        except Exception as ex:
            print("Error processing message:", ex)
            # requeue=False so we don't loop forever on a broken message
            ch.basic_nack(delivery_tag=method.delivery_tag, requeue=False)

    channel.basic_consume(queue=QUEUE_NAME, on_message_callback=on_message)

    print(f"Waiting for messages on '{QUEUE_NAME}'. Press CTRL+C to exit.")
    try:
        channel.start_consuming()
    except KeyboardInterrupt:
        channel.stop_consuming()
    finally:
        connection.close()


if __name__ == "__main__":
    main()