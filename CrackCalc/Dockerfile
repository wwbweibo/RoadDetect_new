FROM tensorflow/tensorflow:latest-py3
ADD CrackCalc /work/CrackCalc
ADD PythonCoreLib /work/CrackCalc
WORKDIR /work
RUN pip install -r CrackCalc/requirements.txt
CMD "python" "CrackCalc/runserver.py"