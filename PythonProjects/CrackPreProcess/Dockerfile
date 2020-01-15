FROM python
ADD . /work
WORKDIR /work
RUN pip install -r requirements.txt -i https://mirrors.aliyun.com/pypi/simple/
CMD "python" "runCrackPreProcessService.py"