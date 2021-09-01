**Paper**

1. **한글제목** : 알파벳으로 덮어씌워진 숫자 인식을 위한 기존 모델의 정확도 분석.
   **영문제목** :  

2. **국문요약** : 
   **영문요약** : 

3. **key words** : Overwritten number, convolution neural network, Image Classification.

4. **서론**

5. 1. **해당 논문을 작성하게 된 이유**
       Image Classification on MNIST의 정확도가 Top Accuracy 99.870를 달성한 모델의 성능을 보고 사람이 작성한 숫자를 분류해내는 정확도가 ILSVRC 대회 역대 우승 알고리즘들과 인식 에러율에서 사람의 숫자 분류 에러율을 5%로 보았을 때 사람보다 약 4.8% 더 높은 정확도를 보이는 것을 알 수 있었다. 앞서 설명한 숫자 분류의 문제는 단순히 필기체를 인식해 분류해내는 것에서 그쳤지만, DACON의 컴퓨터 비전 학습 경진 대회에서는 알파벳으로 가린 숫자의 일부분의 영역을 보아 감추어진 숫자를 예측하는 단순히 인식하여 예측하는 문제보다 더 Task가 높다고 볼 수 있다. 때문에 해당 문제에서도 딥러닝의 정확도가 어떠할지 분석하기 위한 과정이다. 

   2. **DACON 컴퓨터 비전 학습 경진 대회 문제에 대한 설명.**

      1. Digit과 Letter를 합쳐서 만들어진 이미지에서 Letter의 범위을 넘어서는 Digit의 부분의 Pixel의 값을 0으로 낮추어 원본 숫자 이미지의 영역을 감추어 제일 오른쪽의 이미지로 만들어진 데이터를 학습 데이터로 사용합니다. 연두색의 영역은 겹쳐진 숫자의 영역이며, 연녹색의 영역은 Letter의 영역이며 해당 부분에는 감추어진 숫자가 없다는 것을 의미합니다. 이러한 색으로 나타낸 것은 시각화를 하여 편하게 보기위한 임의의 색을 채워둔 것으로 실제로는 grayscale의 이미지가 사용됩니다.

      <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/Task.png?raw=true" alt="Task.png" style="zoom:25%;" />

      2. train_sample, test_sample

         - id : 데이터 id.
         - digit : 가려진 숫자.
         - letter : 숫자를 가리는 알파벳.
         - 0 ~ 784 : 28 by 28 이미지 RGB pixel values.
         - train_sample (1부터 2048)
         - test_sample (2049부터 22528)

      <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/train_sample.png?raw=true" alt="train_sample.png" style="zoom:25%;" />

      <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/test_sample.png?raw=true" alt="test_sample.png" style="zoom:25%;" />

      3. 실제 데이터를 보기 위한 Data visualization.

         1. 사람이 쉽게 예측할 수 있는 train data 예시.

            <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Data%20visualization/Train/1000.png?raw=true" alt="1000.png"/>

            ​															대문자 알파벳 E에 숫자 2가 숨겨져있는 example.

            ![1005.png](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Data%20visualization/Train/1005.png?raw=true)

            ​															소문자 알파벳 e에 숫자 0이 숨겨져있는 example.

         2. 사람이 예측하기 어려운 train data 예시.

            ![106.png](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Data%20visualization/Train/106.png?raw=true)

            ​															소문자 알파벳 r에 숫자 6이 숨겨져있는 example.

            ![1235.png](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Data%20visualization/Train/1235.png?raw=true)

            ​															대문자 알파벳 A에 숫자 8이 숨겨져 있는 example.

         3. 정확도 측정을 위해 주어지는 test data set

            1. 사람이 단번에 예측하기 어려운 Test data set example.

            ![106.png](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Data%20visualization/Test/106.png?raw=true)

            ​																	대문자 알파벳 D에 숫자가 숨겨져있다.

            ![122.png](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Data%20visualization/Test/122.png?raw=true)

            ​																	소문자 알파벳 T에 숫자가 숨겨져있다.

      4. 실제 실험에 사용된 train, test images. (28 by 28)

         1. train data
            1. <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/1001.png?raw=true" alt="1001.png" style="zoom:200%;" />
            2. <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/1006.png?raw=true" alt="1006.png" style="zoom:200%;" />
            3. <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/107.png?raw=true" alt="107.png" style="zoom:200%;" />
            4. <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/1236.png?raw=true" alt="1236.png" style="zoom:200%;" />
         2. test data
            1. <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/2155.png?raw=true" alt="2155.png" style="zoom:200%;" />
            2. <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/2171.png?raw=true" alt="2171.png" style="zoom:200%;" />
         
      5. train data set의 분포.

         가로(0-25) : 알파벳의 종류 (26가지)

         세로(0-9) : 숫자 0부터 9 (10가지)

         26*10 = 260가지로 분포된 train set의 분포 visualization.

         학습하는 과정에서 train data 2048개의 이미지로 test data 20480개를 예측하는데 학습 데이터의 부족이 발생하였고,

         Letter(A or a)로 Digit(3)을 가린 train data의 개수 1개 처럼 Learning data imbalance 문제가 발생 하였다.

         때문에 tensorflow에서 제공하는 Data Argmentation 라이브러리인 ImageDataGenerator를 사용하여서 학습 데이터를 만들어 해당 문위에서 언급한 문제를 해결하였습니다.

      <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/Data%20Distribution.png?raw=true" alt="Data Distribution.png" style="zoom:25%;" />

      6. Data Argmentation - ImageDataGenerator
         1. Data Argmentation's detail Parameter : rotation_range=10, width_shift_range=0.1, height_shift_range=0.1.
            Random하게 생성되는 image data의 이해를 돕기위한 시각화 예시 입니다.
         2. 아래의 이미지에서 볼 수 있든 Random하게 문제의 Pixel 위치를 조정하여 Data Argmentation을 진행하였으며, 
         3. 회전, 플립 등 더 다양하게 Data Argmentation을 할 수 있지만, 진행하게 될 경우 회전의 경우 6,9, 플립의 경우 모든 숫자에 사진에 나타나있는 숫자의 정보를 손실시켜 오히려 Train Data set의 혼란을 야기시켜 진행하지 않았습니다.
         4. Train data set 2048개를 train data(1642개), validation data(406개)로 나누어서 Data Argmentation이 진행되었으며, ImageDataGenerator을 사용하여서 2048개의 이미지를 65536개로 증강하여 학습에는 Train image = 52544, Validataion image = 12992가 사용되었습니다. validation imaga는 train에 사용되지 않고 학습이 진행되었습니다.

         5. <img src="https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Image/IDG.png?raw=true" alt="IDG.png" style="zoom:25%;" />

6. **관련연구**

7. 1. Image Classification. 
   2. Cursive recognition.

8. **진행한 실험** : Individual model Learning을 통해 정확도 추정.

   해당 테스크에 가장 적합한 모델을 찾기 위해서 기존의 Image Classification 분야에서 널리 알려진 모델들을 학습시켜 가장 높은 정확도를 보인는 모델을 기준으로 세부 Parameter를 조정하여 더 정확도를 높이거나, 새로운 방식을 통해 학습을 진행하여 정확도를 높일 예정이다.

   1. 실험 진행한 환경

      Google Colaboratory

      - CPU : 

      - GPU : Tesla P100
      - RAM : 

9. **실험결과 및 분석** : 

   상위-1과 상위-5 정확성은 ImageNet의 검증 데이터셋에 대한 모델의 성능을 가리킵니다.

   깊이란 네트워크의 토폴로지 깊이를 말합니다. 이는 활성화 레이어, 배치 정규화 레이어 등을 포함합니다.

   ```python
   ImageDataGenerator (
   			rescale = 1./255, 
       	validation_split = 0.2,
       	rotation_range = 10,
       	width_shift_range = 0.1,
       	height_shift_range = 0.1)
                  
   Batch_size = 32 (dafault)
   optimizer = Adam (lr=0.002, epsilon=None)
   epochs = 500
   ```

   

   | Public accuracy | Private accuracy |             Model | Top-1 accuracy | Top-5 accuracy |   Parameter | Depth | Default Input Size | Input Size |
   | :-------------: | :--------------: | ----------------: | -------------: | -------------: | ----------: | :---: | :----------------: | :--------: |
   |     0.88235     |     0.86037      |             VGG16 |          0.713 |          0.901 | 138,357,544 |  23   |      224x224       |  224x224   |
   |     0.89215     |     0.88991      |             VGG19 |          0.713 |          0.900 | 143,667,240 |  26   |      224x224       |  224x224   |
   |     0.92156     |     0.90816      |          ResNet50 |          0.749 |          0.921 |  25,636,712 |   -   |      224x224       |  224x224   |
   |     0.92857     |     0.90377      |         ResNet101 |          0.764 |          0.928 |  44,707,176 |   -   |      224x224       |  224x224   |
   |     0.90196     |     0.89568      |         ResNet152 |          0.766 |          0.931 |  60,419,944 |   -   |      224x224       |  224x224   |
   |     0.89215     |     0.90076      |        ResNet50V2 |          0.760 |          0.930 |  25,613,800 |   -   |      224x224       |  224x224   |
   |     0.91666     |     0.91512      |       ResNet101V2 |          0.772 |          0.938 |  44,675,560 |   -   |      224x224       |  224x224   |
   |     0.89705     |     0.89647      |       ResNet152V2 |          0.780 |          0.942 |  60,380,648 |   -   |      224x224       |  224x224   |
   |     0.92156     |     0.91640      |       InceptionV3 |          0.779 |          0.937 |  23,851,784 |  159  |      299x299       |  224x224   |
   |     0.94117     |     0.91408      | InceptionResNetV2 |          0.803 |          0.953 |  55,873,736 |  572  |      299x299       |  224x224   |
   |     0.81862     |     0.82831      |       InceptionV3 |          0.779 |          0.937 |  23,851,784 |  159  |      299x299       |  299x299   |
   |     0.73039     |     0.74758      | InceptionResNetV2 |          0.803 |          0.953 |  55,873,736 |  572  |      299x299       |  299x299   |
   |     0.93137     |   **0.91689**    |       DenseNet121 |          0.750 |          0.923 |   8,062,504 |  121  |      224x224       |  224x224   |
   |     0.92156     |     0.91285      |       DenseNet169 |          0.762 |          0.932 |  14,307,880 |  169  |      224x224       |  224x224   |
   |     0.91666     |     0.90940      |       DenseNet201 |          0.773 |          0.936 |  20,242,984 |  201  |      224x224       |  224x224   |
   |     0.94117     |   **0.91862**    |          Xception |          0.790 |          0.945 |  22,910,480 |  126  |      299x299       |  224x224   |
   |     0.93137     |     0.91009      |          Xception |          0.790 |          0.945 |  22,910,480 |  126  |      299x299       |  299x299   |
   |     0.91666     |     0.89830      |    EfficientNetB0 |          0.763 |          0.932 |        5.3M |   -   |      224x224       |  224x224   |
   |     0.90686     |     0.90032      |    EfficientNetB1 |          0.788 |          0.944 |        7.8M |   -   |      240x240       |  224x224   |
   |     0.92647     |     0.90540      |    EfficientNetB1 |          0.788 |          0.944 |        7.8M |   -   |      240x240       |  240x240   |
   |     0.74019     |     0.72913      |    EfficientNetB2 |          0.798 |          0.949 |        9.2M |   -   |      260x260       |  224x224   |
   |     0.94117     |     0.90930      |    EfficientNetB2 |          0.798 |          0.949 |        9.2M |   -   |      260x260       |  260x260   |
   |     0.92156     |     0.90185      |    EfficientNetB3 |          0.811 |          0.955 |         12M |   -   |      300x300       |  224x224   |
   |     0.92156     |     0.90693      |    EfficientNetB3 |          0.811 |          0.955 |         12M |   -   |      300x300       |  300x300   |
   |     0.91176     |     0.91196      |    EfficientNetB4 |          0.826 |          0.963 |         19M |   -   |      380x380       |  224x224   |
   |        X        |        X         |    EfficientNetB4 |          0.826 |          0.963 |         19M |   -   |      380x380       |  380x380   |
   |     0.93137     |     0.90338      |    EfficientNetB5 |          0.833 |          0.967 |         30M |   -   |      456x456       |  224x224   |
   |        X        |        X         |    EfficientNetB5 |          0.833 |          0.967 |         30M |   -   |      456x456       |  456x456   |
   |     0.95588     |     0.91122      |    EfficientNetB6 |          0.840 |          0.969 |         43M |   -   |      528x528       |  224x224   |
   |        X        |        X         |    EfficientNetB6 |          0.840 |          0.969 |         43M |   -   |      528x528       |  528x528   |
   |        X        |        X         |    EfficientNetB7 |          0.844 |          0.971 |         66M |   -   |      600x600       |  224x224   |
   |        X        |        X         |    EfficientNetB7 |          0.844 |          0.971 |         66M |   -   |      600x600       |  600x600   |

   EfficientNetB4(380x380), EfficientNetB5(456x456), EfficientNetB6(528x528), EfficientNetB7(224x224), EfficientNetB7(600x600)의 경우, Colab pro GPU memory 부족으로 인해서 학습이 불가합니다.

   batch size, Layer수, Filter 갯수, input size를 줄이거나, GPU를 바꾼다 같은 방법들이 있겠지만, 위와 같이 해결하여서 학습을 진행할 경우 지금까지 진행해온 다른 모델 학습의 진행과 다르다고 판단하여 학습을 하지않았습니다.

   추가적으로 EfficientNet B0-B7의 default input size의 경우 224-600의 사이즈를 가지며, 위의 작성된 default input size의 경우, 해당 모델의 최적의 input size를 작성해둔 것입니다.

   Private score가 해당 문제의 test data의 99%를 가지고 체점된 결과라서 Public score보다 훨씬 더 높은 가치를 갖습니다.

   해당 도표에서 알 수 있는 결론은 모델의 최적화된 default input size와 학습의 사용한 image의 input size가 같은 경우 가장 높은 정확도를 보인 모델은 DensetNet121 모델임을 알 수 있고, 

10. **결론**

    1. model ensamble.
    2. Train set의 숫자가 보이는 영역을 모두 합친 이미지를 대상으로 모델을 구성할 필요가 있다.
    3. Validation K-fold.
    4. Parameter optimization.
    5. New labeling. (숫자가 무조건 있는 영역, 숫자가 무조건 없는 영역, 숫자가 있을 수 있는 영역)

11. **참고문헌**

    ​		Image Classification on MNIST

    1. https://paperswithcode.com/sota/image-classification-on-mnist?metric=Accuracy

       Computer vision large-scale visual recognition challenge (ILSVRC)

    2. https://image-net.org/challenges/LSVRC/index.php

       ILSVRC 대회 역대 우승 알고리즘들과 인식 에러율

    3. https://bskyvision.com/425

       TensorFlow Core v2.6.0 Module : tf.keras.applications

    4. https://www.tensorflow.org/api_docs/python/tf/keras/applications

       Keras Documentation : Applications.

    5. https://keras.io/ko/applications/



애플워치와 같은 작은 공간에 필기체 인식을 위한 글자 겹쳐쓰기, 겹쳐쓰기 인식

http://www.selvy.ai/pen,

https://handwriting.selvasai.com/text/handwriting_intro.html



### Memo.

먼저 EDA를 진행할 때 좀 더 쉬운 이미지를 사용해서 설명해야하고,

숫자를 문자로 가리고 남은 부분을 날렸다는 것에 대한 설명이 추가로 필요하고 근본적으로 해당 문제를 왜 진행했는지에 대해서 알아봐야할 필요가 있다. ( 왜 문자로 숫자를 가리고 가려진 숫자를 찾아야하는지? )

- 잘 보이지 않는 번호판을 정확하게 인식하는 문제 
  ( 일부분의 정보를 가지고 숫자를 유추해야하는 관점에서는 같은 의미를 같는다고 생각한다. )

- 작은 공간에 필기체 인식을 위한 글자 겹쳐쓰기. (겹쳐쓰기 인식)

혹은 더 적은 양의 정보를 가지고도 숫자를 인식해야한다보다 겹쳐진 글자를 인식해야하는 문제가 더욱 유사하다고 볼 수 있다.
