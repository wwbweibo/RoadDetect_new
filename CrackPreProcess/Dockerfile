FROM python
ADD /CrackPreProcess /work/CrackPreProcess
ADD /PythonCoreLib /work/PythonCoreLib
WORKDIR /work
RUN pip install -r CrackPreProcess/requirements.txt
CMD "python" "CrackPreProcess/runserver.py"