## Data input size의 경향성
> Data input size의 증가, 감소에 따라 정확도에 미치는 영향을 파악하기 위한 실험.

```python
model = DenseNet121
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

Data input size

| Input size | acc(one) | acc(two) | acc(thr) | acc(four) | acc(five) | Avg  |
| ---------- | -------- | -------- | -------- | --------- | --------- | ---- |
| 32x32      |          |          |          |           |           |      |
| 64x64      |          |          |          |           |           |      |
| 128x128    |          |          |          |           |           |      |
| 224x224    |          |          |          |           |           |      |
| 256x256    |          |          |          |           |           |      |
| 299x299    |          |          |          |           |           |      |

