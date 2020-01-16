FROM tensorflow/tensorflow:latest-py3
ADD . /work
WORKDIR /work
RUN apt update && apt install libsm6 libxrender1 libxext6 -y
RUN pip install -r requirements.txt -i https://mirrors.aliyun.com/pypi/simple/
CMD "python" "runCrackCalcService.py"