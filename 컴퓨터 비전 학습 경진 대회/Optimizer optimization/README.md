## Optimizer optimization

Baseline parameter.

```python
model = DenseNet121 (input size = 224 x 224)
datagen = ImageDataGenerator(
                             rescale=1./255, 
                             validation_split=0.2,
                             rotation_range=10,
                             width_shift_range=0.1,
                             height_shift_range=0.1)
                             
Batch_size = 32 (dafault)
epochs = 500
```

[`class Adadelta`](https://www.tensorflow.org/api_docs/python/tf/keras/optimizers/Adadelta): Optimizer that implements the Adadelta algorithm.

[`class Adagrad`](https://www.tensorflow.org/api_docs/python/tf/keras/optimizers/Adagrad): Optimizer that implements the Adagrad algorithm.

[`class Adam`](https://www.tensorflow.org/api_docs/python/tf/keras/optimizers/Adam): Optimizer that implements the Adam algorithm.

[`class Adamax`](https://www.tensorflow.org/api_docs/python/tf/keras/optimizers/Adamax): Optimizer that implements the Adamax algorithm.

[`class Ftrl`](https://www.tensorflow.org/api_docs/python/tf/keras/optimizers/Ftrl): Optimizer that implements the FTRL algorithm.

[`class Nadam`](https://www.tensorflow.org/api_docs/python/tf/keras/optimizers/Nadam): Optimizer that implements the NAdam algorithm.

[`class Optimizer`](https://www.tensorflow.org/api_docs/python/tf/keras/optimizers/Optimizer): Base class for Keras optimizers.

[`class RMSprop`](https://www.tensorflow.org/api_docs/python/tf/keras/optimizers/RMSprop): Optimizer that implements the RMSprop algorithm.

[`class SGD`](https://www.tensorflow.org/api_docs/python/tf/keras/optimizers/SGD): Gradient descent (with momentum) optimizer.

| Optimizer | Acc(One) | Acc(Two) | Acc(Three) | Acc(Four) | Acc(Five) | Average |
| --------- | -------- | -------- | ---------- | --------- | --------- | ------- |
| Adadelta  |          |          |            |           |           |         |
| Adagrad   |          |          |            |           |           |         |
| Adam      |          |          |            |           |           |         |
| Adamax    |          |          |            |           |           |         |
| Ftrl      |          |          |            |           |           |         |
| Nadam     |          |          |            |           |           |         |
| Optimizer |          |          |            |           |           |         |
| RMSprop   |          |          |            |           |           |         |
| SGD       |          |          |            |           |           |         |
