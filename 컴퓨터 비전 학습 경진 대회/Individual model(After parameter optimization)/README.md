## Individual model - 32 models.

[tensorflow keras application model Link](https://www.tensorflow.org/api_docs/python/tf/keras/applications), [Keras applications documentation](https://keras.io/ko/applications/)

> Train data set 2048개를 train data( 1,900개 ), validation data( 148개 )로 나누어서 학습을 진행되었으며, 
> ImageDataGenerator을 사용하여서 Data Argmentation을 진행하여 학습에는 Train image = 15,200, Validataion image = 1,184이 사용되었습니다. 

> 상위-1과 상위-5 정확성은 ImageNet의 검증 데이터셋에 대한 모델의 성능을 가리킵니다.
>
> 깊이란 네트워크의 토폴로지 깊이를 말합니다. 이는 활성화 레이어, 배치 정규화 레이어 등을 포함합니다. 

```python
image size = 224.
Batch_size = 8.
optimizer = Adam.
epochs = 500.
ImageDataGenerator (
		rescale = 1./255, 
		validation_split = 0.075,
		rotation_range = 15,
		width_shift_range = 0.00,
		height_shift_range = 0.05 )
```


|             Model | Private acc | Private acc | Private acc | Private acc | Private acc | AVG. | Input Size | Input Size |
| ----------------: | ----------- | ----------- | ----------- | ----------- | ----------- | ---- | :--------: | :--------: |
|             VGG16 |             |             |             |             |             |      |    224     |    224     |
|             VGG19 |             |             |             |             |             |      |    224     |    224     |
|          ResNet50 |             |             |             |             |             |      |    224     |    224     |
|         ResNet101 |             |             |             |             |             |      |    224     |    224     |
|         ResNet152 |             |             |             |             |             |      |    224     |    224     |
|        ResNet50V2 |             |             |             |             |             |      |    224     |    224     |
|       ResNet101V2 |             |             |             |             |             |      |    224     |    224     |
|       ResNet152V2 |             |             |             |             |             |      |    224     |    224     |
|       InceptionV3 |             |             |             |             |             |      |    299     |    224     |
| InceptionResNetV2 |             |             |             |             |             |      |    299     |    224     |
|       InceptionV3 |             |             |             |             |             |      |    299     |    299     |
| InceptionResNetV2 |             |             |             |             |             |      |    299     |    299     |
|       DenseNet121 |             |             |             |             |             |      |    224     |    224     |
|       DenseNet169 |             |             |             |             |             |      |    224     |    224     |
|       DenseNet201 |             |             |             |             |             |      |    224     |    224     |
|          Xception |             |             |             |             |             |      |    299     |    224     |
|          Xception |             |             |             |             |             |      |    299     |    299     |
|    EfficientNetB0 |             |             |             |             |             |      |    224     |    224     |
|    EfficientNetB1 |             |             |             |             |             |      |    240     |    224     |
|    EfficientNetB1 |             |             |             |             |             |      |    240     |    240     |
|    EfficientNetB2 |             |             |             |             |             |      |    260     |    224     |
|    EfficientNetB2 |             |             |             |             |             |      |    260     |    260     |
|    EfficientNetB3 |             |             |             |             |             |      |    300     |    224     |
|    EfficientNetB3 |             |             |             |             |             |      |    300     |    300     |
|    EfficientNetB4 |             |             |             |             |             |      |    380     |    224     |
|    EfficientNetB4 |             |             |             |             |             |      |    380     |    380     |
|    EfficientNetB5 |             |             |             |             |             |      |    456     |    224     |
|    EfficientNetB5 |             |             |             |             |             |      |    456     |    456     |
|    EfficientNetB6 |             |             |             |             |             |      |    528     |    224     |
|    EfficientNetB6 |             |             |             |             |             |      |    528     |    528     |
|    EfficientNetB7 |             |             |             |             |             |      |    600     |    224     |
|    EfficientNetB7 |             |             |             |             |             |      |    600     |    600     |

> EfficientNetB4(380x380), EfficientNetB5(456x456), EfficientNetB6(528x528), EfficientNetB7(224x224), EfficientNetB7(600x600)의 경우, Colab pro GPU memory 부족으로 인해서 학습이 불가합니다.
>
> batch size, Layer수, Filter 갯수, input size를 줄이거나, GPU를 바꾼다 같은 방법들이 있겠지만, 
>
> 위와 같이 해결하여서 학습을 진행할 경우 지금까지 진행하온 학습의 Parameter와 다르기때문에 진행을 보류하였습니다.

> 추가적으로 EfficientNet B0-B7의 default input size의 경우 224-600의 사이즈를 가지며,
> 위의 작성된 default input size의 경우, 해당 모델의 최적의 input size를 작성해둔 것입니다.
