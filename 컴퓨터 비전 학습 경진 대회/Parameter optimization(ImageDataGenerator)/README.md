## Parameter optimization(ImageDataGenerator)

Baseline - Individual model(Before parameter optimization)

```python
datagen = ImageDataGenerator(rescale=1./255, 
                             validation_split=0.2,
                             rotation_range=10,
                             width_shift_range=0.1,
                             height_shift_range=0.1)
                             
Batch_size = 32 (dafault)
```

validation_split(rotation_range=10, width_shift_range=0.1, height_shift_range=0.1)

| validation_split | Acc(One) | Acc(Two) | Acc(Three) | Acc(Four) | Acc(Five) | Average |
| ---------------- | -------- | -------- | ---------- | --------- | --------- | ------- |
| 0.000            |  [0.90683](https://github.com/d9249/DACON/edit/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/README.md)        |          |            |           |           |         |
| 0.050            |   [0.92153](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/validation_split/0.05_epochs%3D500_ResNet50(Public-0.91176%2C%20Private-0.92153).ipynb)       |   [0.90318](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/validation_split/0.05_epochs%3D500_ResNet50(Public-0.92647%2C%20Private-0.90318).ipynb)       |   [0.91517](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/validation_split/0.05_epochs%3D500_ResNet50(Public-0.93137%2C%20Private-0.91517).ipynb)         |   [0.92602](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/validation_split/0.05_epochs%3D500_ResNet50(Public-0.93137%2C%20Private-0.92602).ipynb)        |  [0.92424](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/validation_split/0.05_epochs%3D500_ResNet50(Public-0.94607%2C%20Private-0.92424).ipynb)         |    0.918028     |
| 0.075            |  [0.90723](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/validation_split/0.075_epochs%3D500_ResNet50(Public-0.91176%2C%20Private-0.90723).ipynb)        |          |            |           |           |         |
| 0.100            | [0.91906](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/validation_split/0.1_epochs%3D500_ResNet50(Public-0.92156%2C%20Private-0.91906).ipynb)         |          |            |           |           |         |
| 0.150            |          |          |            |           |           |         |
| 0.200            |   [0.91699](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/validation_split/0.2_epochs%3D500_ResNet50(Public-0.90686%2C%20Private-0.91699).ipynb)       |          |            |           |           |         |

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
| 0.05        | [0.89623](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/shear_range/0.05_ResNet50(Public-0.89215%2C%20Private-0.89623).ipynb)         |          |            |           |           |         |
| 0.10        | [0.89904](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/shear_range/0.1_ResNet50(Public-0.91176%2C%20Private-0.89904).ipynb)         |          |            |           |           |         |
| 0.15        |          |          |            |           |           |         |
| 0.20        |          |          |            |           |           |         |
| 0.25        |          |          |            |           |           |         |
| 0.30        | [0.86738](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/shear_range/0.3_ResNet50(Public-0.87254%2C%20Private-0.86738).ipynb)         |          |            |           |           |         |

batch_size

| batch_size | Acc(One) | Acc(Two) | Acc(Three) | Acc(Four) | Acc(Five) | Average |
| ---------- | -------- | -------- | ---------- | --------- | --------- | ------- |
| 4          |          |          |            |           |           |         |
| 8          |          |          |            |           |           |         |
| 16         |  [0.91798](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/batch_size/16_ResNet50(Public-0.93137%2C%20Private-0.91798).ipynb)        | [0.]()         |  [0.]()          |  [0.]()         |   [0.]()        |  [avg]()       |
| 32         | [0.91699](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/validation_split/0.2_epochs%3D500_ResNet50(Public-0.90686%2C%20Private-0.91699).ipynb)         |          |            |           |           |         |
| 64         |[0.89139](https://github.com/d9249/DACON/edit/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Parameter%20optimization(ImageDataGenerator)/README.md)     |          |            |           |           |         |
