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
| 0.000            |          |          |            |           |           |         |
| 0.050            |          |          |            |           |           |         |
| 0.075            |          |          |            |           |           |         |
| 0.100            |          |          |            |           |           |         |
| 0.150            |          |          |            |           |           |         |
| 0.200            |          |          |            |           |           |         |

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
