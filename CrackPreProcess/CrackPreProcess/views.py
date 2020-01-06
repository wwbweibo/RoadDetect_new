"""
Routes and views for the flask application.
"""

from datetime import datetime
from flask import render_template, request
from CrackPreProcess import app
from CrackPreProcess.Kafka  import Client
from CrackPreProcess.Service.PreProcessService import PreProcessService

@app.route('/')
def home():
    return "anc"

@app.route("/preprocess", methods=['POST'])
def PreProcess():
    try:
        imageb64data = request.form['imageb64data']
        # basic validation
        if imageb64data is None or len(imageb64data) <=0:
            raise("the input data can not be None or empty")
        service = PreProcessService(imageb64data)
        shape = service.ExecuteWorkFlow()
        return '{%d,%d,%d}' % (shape[0], shape[1], shape[2])
    except e:
        print(e)


