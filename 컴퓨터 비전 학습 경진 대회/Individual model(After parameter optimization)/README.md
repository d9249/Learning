## Individual model - 32 models.

[tensorflow keras application model Link](https://www.tensorflow.org/api_docs/python/tf/keras/applications), [Keras applications documentation](https://keras.io/ko/applications/)

> Train data set 2048개를 train data(개), validation data(개)로 나누어서 학습을 진행되었으며, ImageDataGenerator을 사용하여서 Data Argmentation을 진행하여 학습에는 Train image = , Validataion image = 가 사용되었습니다. 

> 상위-1과 상위-5 정확성은 ImageNet의 검증 데이터셋에 대한 모델의 성능을 가리킵니다.
>
> 깊이란 네트워크의 토폴로지 깊이를 말합니다. 이는 활성화 레이어, 배치 정규화 레이어 등을 포함합니다. 

```python
datagen = ImageDataGenerator(rescale=1./255, 
                             validation_split=,
                             rotation_range=,
                             width_shift_range=,
                             height_shift_range=)

Batch_size = 
```


| Public accuracy | Private accuracy |                       Model (Link : Experiment Results File) | Top-1 accuracy | Top-5 accuracy |   Parameter | Depth |                         Result Link                          | Default Input Size | Input Size |
| :-------------: | :--------------: | -----------------------------------------------------------: | -------------: | -------------: | ----------: | :---: | :----------------------------------------------------------: | :----------------: | :--------: |
|                 |                  |                                                        [VGG16]() |          0.713 |          0.901 | 138,357,544 |  23   |                             [Link]()                             |      224x224       |  224x224   |
|                 |                  |                                                        [VGG19]() |          0.713 |          0.900 | 143,667,240 |  26   |                             [Link]()                             |      224x224       |  224x224   |
|                 |                  | [ResNet50]() |          0.749 |          0.921 |  25,636,712 |   -   | [Link]() |      224x224       |  224x224   |
|                 |                  | [ResNet101]() |          0.764 |          0.928 |  44,707,176 |   -   | [Link]() |      224x224       |  224x224   |
|                 |                  | [ResNet152]() |          0.766 |          0.931 |  60,419,944 |   -   | [Link]() |      224x224       |  224x224   |
|                 |                  | [ResNet50V2]() |          0.760 |          0.930 |  25,613,800 |   -   | [Link]() |      224x224       |  224x224   |
|                 |                  | [ResNet101V2]() |          0.772 |          0.938 |  44,675,560 |   -   | [Link]() |      224x224       |  224x224   |
|                 |                  | [ResNet152V2]() |          0.780 |          0.942 |  60,380,648 |   -   | [Link]() |      224x224       |  224x224   |
|                 |                  | [InceptionV3]() |          0.779 |          0.937 |  23,851,784 |  159  | [Link]() |      299x299       |  224x224   |
|                 |                  | [InceptionResNetV2]() |          0.803 |          0.953 |  55,873,736 |  572  | [Link]() |      299x299       |  224x224   |
|                 |                  | [InceptionV3]() |          0.779 |          0.937 |  23,851,784 |  159  | [Link]() |      299x299       |  299x299   |
|                 |                  | [InceptionResNetV2]() |          0.803 |          0.953 |  55,873,736 |  572  | [Link]() |      299x299       |  299x299   |
|                 |                  | [DenseNet121]() |          0.750 |          0.923 |   8,062,504 |  121  | [Link]() |      224x224       |  224x224   |
|                 |                  | [DenseNet169]() |          0.762 |          0.932 |  14,307,880 |  169  | [Link]() |      224x224       |  224x224   |
|                 |                  | [DenseNet201]() |          0.773 |          0.936 |  20,242,984 |  201  | [Link]() |      224x224       |  224x224   |
|                 |                  | [Xception]() |          0.790 |          0.945 |  22,910,480 |  126  | [Link]() |      299x299       |  224x224   |
|                 |                  | [Xception]() |          0.790 |          0.945 |  22,910,480 |  126  | [Link]() |      299x299       |  299x299   |
|                 |                  | [EfficientNetB0]() |          0.763 |          0.932 |        5.3M |   -   | [Link]() |      224x224       |  224x224   |
|                 |                  | [EfficientNetB1]() |          0.788 |          0.944 |        7.8M |   -   | [Link]() |      240x240       |  224x224   |
|                 |                  | [EfficientNetB1]() |          0.788 |          0.944 |        7.8M |   -   | [Link]() |      240x240       |  240x240   |
|                 |                  | [EfficientNetB2]() |          0.798 |          0.949 |        9.2M |   -   | [Link]() |      260x260       |  224x224   |
|                 |                  | [EfficientNetB2]() |          0.798 |          0.949 |        9.2M |   -   | [Link]() |      260x260       |  260x260   |
|                 |                  | [EfficientNetB3]() |          0.811 |          0.955 |         12M |   -   | [Link]() |      300x300       |  224x224   |
|                 |                  | [EfficientNetB3]() |          0.811 |          0.955 |         12M |   -   | [Link]() |      300x300       |  300x300   |
|                 |                  | [EfficientNetB4]() |          0.826 |          0.963 |         19M |   -   | [Link]() |      380x380       |  224x224   |
|        X        |        X         |                                               EfficientNetB4 |          0.826 |          0.963 |         19M |   -   |                              -                               |      380x380       |  380x380   |
|                 |                  | [EfficientNetB5]() |          0.833 |          0.967 |         30M |   -   | [Link]() |      456x456       |  224x224   |
|        X        |        X         |                                               EfficientNetB5 |          0.833 |          0.967 |         30M |   -   |                              -                               |      456x456       |  456x456   |
|                 |                  | [EfficientNetB6]() |          0.840 |          0.969 |         43M |   -   | [Link]() |      528x528       |  224x224   |
|        X        |        X         |                                               EfficientNetB6 |          0.840 |          0.969 |         43M |   -   |                              -                               |      528x528       |  528x528   |
|        X        |        X         |                                               EfficientNetB7 |          0.844 |          0.971 |         66M |   -   |                              -                               |      600x600       |  224x224   |
|        X        |        X         |                                               EfficientNetB7 |          0.844 |          0.971 |         66M |   -   |                              -                               |      600x600       |  600x600   |

> EfficientNetB4(380x380), EfficientNetB5(456x456), EfficientNetB6(528x528), EfficientNetB7(224x224), EfficientNetB7(600x600)의 경우, Colab pro GPU memory 부족으로 인해서 학습이 불가합니다.
>
> batch size, Layer수, Filter 갯수, input size를 줄이거나, GPU를 바꾼다 같은 방법들이 있겠지만, 
>
> 위와 같이 해결하여서 학습을 진행할 경우 지금까지 진행하온 학습의 Parameter와 다르기때문에 진행을 보류하였습니다.

> 추가적으로 EfficientNet B0-B7의 default input size의 경우 224-600의 사이즈를 가지며,
> 위의 작성된 default input size의 경우, 해당 모델의 최적의 input size를 작성해둔 것입니다.
