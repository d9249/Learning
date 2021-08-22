## Parameter optimization(ImageDataGenerator)

Baseline - Individual model(Before parameter optimization)

```python
model = DenseNet121 (input size = 224x224)
datagen = ImageDataGenerator(
                             rescale=1./255, 
                             validation_split=0.2,
                             rotation_range=10,
                             width_shift_range=0.1,
                             height_shift_range=0.1)
                             
Batch_size = 32 (dafault)
optimizer = Adam(lr=0.002, epsilon=None)
epochs = 500
```

validation_split(rotation_range=10, width_shift_range=0.1, height_shift_range=0.1)

| validation_split | Acc(One)                                                     | Acc(Two)                                                     | Acc(Three)                                                   | Acc(Four)                                                    | Acc(Five)                                                    | Average      |
| ---------------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------ |
| 0.000            | [**0.89618**](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/Validation_split/Validation_split_0.000_1_DenseNet121(public-0.92647%2C%20private-0.89618).ipynb) | [**0.90491**](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/Validation_split/Validation_split_0.000_2_DenseNet121(public-0.92647%2C%20private-0.90491).ipynb) | [**0.92237**](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/Validation_split/Validation_split_0.000_3_DenseNet121(public-0.94117%2C%20private-0.92237).ipynb) | [**0.88617**](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/Validation_split/Validation_split_0.000_4_DenseNet121(public-0.91176%2C%20private-0.88617).ipynb) | [**0.91448**](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/Validation_split/Validation_split_0.000_5_DenseNet121(public-0.94117%2C%20private-0.91448).ipynb) | **0.904822** |
| 0.050            |                                                              | [**0.92168**](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/Validation_split/Validation_split_0.050_2_DenseNet121(public-0.93627%2C%20private-0.92168).ipynb) |                                                              |                                                              |                                                              |              |
| 0.075            |                                                              |                                                              |                                                              |                                                              |                                                              |              |
| 0.100            |                                                              |                                                              |                                                              |                                                              |                                                              |              |
| 0.150            |                                                              |                                                              |                                                              |                                                              |                                                              |              |
| 0.200            |                                                              |                                                              |                                                              |                                                              |                                                              |              |

rotation_range(validation_split=0.2, width_shift_range=0.1, height_shift_range=0.1)

| rotation_range | Acc(One) | Acc(Two) | Acc(Three) | Acc(Four) | Acc(Five) | Average |
| -------------- | -------- | -------- | ---------- | --------- | --------- | ------- |
| 0              |          |          |            |           |           |         |
| 5              |          |          |            |           |           |         |
| 10             |          |          |            |           |           |         |
| 15             |          |          |            |           |           |         |
| 20             |          |          |            |           |           |         |
| 25             |          |          |            |           |           |         |
| 30             |          |          |            |           |           |         |

width_shift_range(validation_split=0.2, rotation_range=10, height_shift_range=0.1)

| width_shift_range | Acc(One) | Acc(Two) | Acc(Three) | Acc(Four) | Acc(Five) | Average |
| ----------------- | -------- | -------- | ---------- | --------- | --------- | ------- |
| 0.00              |          |          |            |           |           |         |
| 0.05              |          |          |            |           |           |         |
| 0.10              |          |          |            |           |           |         |
| 0.15              |          |          |            |           |           |         |
| 0.20              |          |          |            |           |           |         |
| 0.25              |          |          |            |           |           |         |
| 0.30              |          |          |            |           |           |         |

height_shift_range(validation_split=0.2, rotation_range=10, width_shift_range=0.1)

| height_shift_range | Acc(One) | Acc(Two) | Acc(Three) | Acc(Four) | Acc(Five) | Average |
| ------------------ | -------- | -------- | ---------- | --------- | --------- | ------- |
| 0.00               |          |          |            |           |           |         |
| 0.05               |          |          |            |           |           |         |
| 0.10               |          |          |            |           |           |         |
| 0.15               |          |          |            |           |           |         |
| 0.20               |          |          |            |           |           |         |
| 0.25               |          |          |            |           |           |         |
| 0.30               |          |          |            |           |           |         |

shear_range(validation_split=0.2, rotation_range=10, width_shift_range=0.1, height_shift_range=0.1)

| shear_range | Acc(One) | Acc(Two) | Acc(Three) | Acc(Four) | Acc(Five) | Average |
| ----------- | -------- | -------- | ---------- | --------- | --------- | ------- |
| 0.05        |          |          |            |           |           |         |
| 0.10        |          |          |            |           |           |         |
| 0.15        |          |          |            |           |           |         |
| 0.20        |          |          |            |           |           |         |
| 0.25        |          |          |            |           |           |         |
| 0.30        |          |          |            |           |           |         |

batch_size

| batch_size | Acc(One) | Acc(Two) | Acc(Three) | Acc(Four) | Acc(Five) | Average |
| ---------- | -------- | -------- | ---------- | --------- | --------- | ------- |
| 4           |          |          |            |           |           |         |
| 8           |          |          |            |           |           |         |
| 16         |          | [0.]()         |  [0.]()          |  [0.]()         |   [0.]()        |  [avg]()       |
| 32         |          |          |            |           |           |         |
| 64         |         |          |            |           |           |         |
