FROM python:3.9

RUN pip install pandas pm4py

WORKDIR /app

COPY 6abb4048-26df-417c-a191-f62bf3410801.py .

CMD ["python", "6abb4048-26df-417c-a191-f62bf3410801.py", "/app/shared/ec0dac50-db58-4707-acb2-7fb37b9f5c72/InputFiles/ff12e711-c39b-4289-86d0-b01914e0127f", "/app/shared/ec0dac50-db58-4707-acb2-7fb37b9f5c72/OutputFiles/9760fcef-0e97-4f68-8ab4-d2f007cd8e84.*"]