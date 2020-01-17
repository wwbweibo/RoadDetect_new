"""
Routes and views for the flask application.
"""

from datetime import datetime
from flask import render_template, request
from CrackPreProcess import app, service

@app.route('/')
def home():
    return "index"

@app.route("/preprocess", methods=['POST'])
def PreProcess():
    try:
        imageb64data = request.form['imageb64data']
        # basic validation
        if imageb64data is None or len(imageb64data) <=0:
            raise("the input data can not be None or empty")
        service.execute_workflow(imageb64data, datatype='HttpPost')
        return '123'
    except Exception as e:
        return e