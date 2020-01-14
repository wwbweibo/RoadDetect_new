import numpy as np
import keras
from keras.models import Sequential
from keras.layers import Dense, Dropout, Flatten, BatchNormalization, Conv2DTranspose, Reshape
from keras.layers import Conv2D, MaxPooling2D
from keras.optimizers import SGD

def gen_model():
    model = Sequential()
    model.add(Conv2D(32, (3, 3), activation='relu',
                     padding='same', input_shape=(16, 16, 1)))
    model.add(Conv2D(32, (3, 3), activation='relu', padding='same'))
    model.add(BatchNormalization(momentum=0.8))
    model.add(MaxPooling2D((2, 2)))

    model.add(Conv2D(32, (3, 3), activation='relu', padding='same'))
    model.add(Conv2D(32, (3, 3), activation='relu', padding='same'))
    model.add(BatchNormalization(momentum=0.8))
    model.add(MaxPooling2D((2, 2)))

    model.add(Conv2D(32, (3, 3), activation='relu', padding='same'))
    model.add(Conv2D(32, (3, 3), activation='relu', padding='same'))
    model.add(BatchNormalization(momentum=0.8))
    model.add(MaxPooling2D((2, 2)))

    model.add(Flatten())
    # model.add(Dense(100, activation='relu'))
    model.add(Dense(10, activation='relu', name="encoder_output"))
    # model.add(Dense(100, activation='relu'))
    # model.add(Dense(512, activation='relu'))
    # model.add(Reshape((2, 2, 128)))
# ------------------------------
    model.add(Dense(128, activation='relu'))
    model.add(Reshape((2,2,32)))

    model.add(Conv2DTranspose(128, (3, 3), strides=2,
                              activation='relu', padding='same'))
    model.add(Conv2DTranspose(64, (3, 3), strides=2,
                              activation='relu', padding='same'))
    model.add(Conv2DTranspose(32, (3, 3), strides=2,
                              activation='relu', padding='same'))
    model.add(Conv2D(1, (3, 3), padding='same', activation='relu'))
    model.compile(optimizer='sgd', loss='mae')
    return model
