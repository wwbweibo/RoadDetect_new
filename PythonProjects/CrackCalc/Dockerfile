FROM tensorflow/tensorflow:latest-py3
ADD . /work
WORKDIR /work
RUN pip install -r requirements.txt -i https://mirrors.aliyun.com/pypi/simple/
CMD "python" "runCrackCalcService.py"