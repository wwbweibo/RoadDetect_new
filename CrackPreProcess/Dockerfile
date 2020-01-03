#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat 
FROM python
RUN mkdir /work
WORKDIR /work
COPY requirements.txt /work/
RUN pip install -r requirements.txt
EXPOSE 5555
CMD python runserver.py