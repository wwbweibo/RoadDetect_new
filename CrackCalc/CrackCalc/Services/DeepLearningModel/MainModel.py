import sklearn.metrics
from Cluster import ClusteringLayer
from new_ae import gen_model
import numpy as np
from keras.layers import Input, Flatten, Dense
from keras.models import Model
from sklearn.cluster import KMeans

import time

class URD:
    def __init__(self, ae_weights, URD_weights=None):
        '''
        URD Model 
        param ae_weights: the path to auto_encoder's weight file 
        '''
        self.ae = gen_model()
        self.ae.load_weights(ae_weights)
        self.encoder = Model(self.ae.inputs, self.ae.get_layer('encoder_output').output)
        model_input = Input(shape=(16,16,1))
        encoder_output = self.encoder(model_input)
        clustering = ClusteringLayer(2, name='clustering')(encoder_output)
        self.model = Model(model_input, clustering)
        self.model.compile(optimizer='sgd', loss="kld")
        self.model.summary()
        if URD_weights is not None:
            self.model.load_weights(URD_weights)

    def get_model(self):
        return self.model, self.ae, self.encoder

    def target_distribution(self, q):
        weight = q ** 2 / q.sum(0)
        return (weight.T / weight.sum(1)).T

if __name__ == "__main__":
    urd = URD('models/ae_0112_new.h5')
    urd.train()
    urd.speed_test()