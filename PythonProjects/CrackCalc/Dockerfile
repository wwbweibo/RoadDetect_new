FROM tensorflow/tensorflow:latest-py3
ADD . /work
WORKDIR /work
RUN pip install -r requirements.txt
CMD "python" "runCrackCalcService.py"