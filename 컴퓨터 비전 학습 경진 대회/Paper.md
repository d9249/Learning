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

      1. Digit과 Letter를 합쳐서 만들어진 이미지에서 Letter의 범위을 넘어서는 Digit의 부분의 Pixel의 값을 0으로 낮추어 원본 숫자 이미지의 영역을 감추어 제일 오른쪽의 이미지로 만들어진 데이터를 학습 데이터로 사용합니다. 연두색의 영역은 겹쳐진 숫자의 영역이며, 연녹색의 영역은 Letter의 영역이며 해당 부분에는 감추어진 숫자가 없다는 것을 의미합니다.

      <img src="/Users/mean/Library/Application Support/typora-user-images/image-20210830010258353.png" alt="image-20210830010258353" style="zoom: 20%;" />

      2. train_sample, test_sample

         - id : 데이터 id.

         - digit : 가려진 숫자.
         - letter : 숫자를 가리는 알파벳.
         - 0 ~ 784 : 28 by 28 이미지 RGB pixel values.

      <img src="/Users/mean/Library/Application Support/typora-user-images/image-20210830010329402.png" alt="image-20210830010329402" style="zoom: 20%;" />

      <img src="/Users/mean/Library/Application Support/typora-user-images/image-20210830010337445.png" alt="image-20210830010337445" style="zoom:20%;" />

      3. 실제 데이터를 보기 위한 Data visualization.

      <img src="/Users/mean/Library/Application Support/typora-user-images/image-20210830011823604.png" alt="image-20210830011823604" style="zoom:20%;" />

      <img src="/Users/mean/Library/Application Support/typora-user-images/image-20210830012615450.png" alt="image-20210830012615450" style="zoom:20%;" />

      4. train data set의 분포.

      <img src="/Users/mean/Library/Application Support/typora-user-images/image-20210830010354601.png" alt="image-20210830010354601" style="zoom:15%;" />

      

6. **관련연구**

7. 1. Image Classification. 
   2. Cursive recognition.

8. **진행한 실험** : Individual model Learning을 통해 정확도 추정.

   해당 테스크에 가장 적합한 모델을 찾기 위해서 기존의 Image Classification 분야에서 널리 알려진 모델들을 학습시켜 가장 높은 정확도를 보인는 모델을 기준으로 세부 Parameter를 조정하여 더 정확도를 높이거나, 새로운 방식을 통해 학습을 진행하여 정확도를 높일 예정이다.

   1. 실험 진행한 환경

      Google Colaboratory

      - Tesla P100

   2. Data Argmentation's detail Parameter : rotation_range=10, width_shift_range=0.1, height_shift_range=0.1.

   <img src="/Users/mean/Library/Application Support/typora-user-images/image-20210830010506726.png" alt="image-20210830010506726" style="zoom: 25%;" />

   

9. **실험결과 및 분석** : 

   | Public accuracy | Private accuracy |                       Model (Link : Experiment Results File) | Top-1 accuracy | Top-5 accuracy |   Parameter | Depth |                         Result Link                          | Default Input Size | Input Size |
   | :-------------: | :--------------: | -----------------------------------------------------------: | -------------: | -------------: | ----------: | :---: | :----------------------------------------------------------: | :----------------: | :--------: |
   |     0.88235     |     0.86037      |                                                        VGG16 |          0.713 |          0.901 | 138,357,544 |  23   |                             Link                             |      224x224       |  224x224   |
   |     0.89215     |     0.88991      |                                                        VGG19 |          0.713 |          0.900 | 143,667,240 |  26   |                             Link                             |      224x224       |  224x224   |
   |     0.92156     |     0.90816      | [ResNet50](https://drive.google.com/file/d/1wPau-IXTl15NRIDe6xSjcMNrYT3jGP8R/view?usp=sharing) |          0.749 |          0.921 |  25,636,712 |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet50(public-0.92156%2C%20private-0.90816).ipynb) |      224x224       |  224x224   |
   |     0.92857     |     0.90377      | [ResNet101](https://drive.google.com/file/d/1L98_1aydEzZuRPfl7CYadLJ9I6K_eM55/view?usp=sharing) |          0.764 |          0.928 |  44,707,176 |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet101(public-0.92857%2C%20private-0.90377).ipynb) |      224x224       |  224x224   |
   |     0.90196     |     0.89568      | [ResNet152](https://drive.google.com/file/d/1PSrJRKc4dd8R2FGhifNOku3JkYsl-Ikb/view?usp=sharing) |          0.766 |          0.931 |  60,419,944 |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet152(public-0.90196%2C%20private-0.89568).ipynb) |      224x224       |  224x224   |
   |     0.89215     |     0.90076      | [ResNet50V2](https://drive.google.com/file/d/1qv9lv15CQFarucihFBw0eI6BigjXu-gh/view?usp=sharing) |          0.760 |          0.930 |  25,613,800 |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet50V2(public-0.89215%2C%20private-0.90076).ipynb) |      224x224       |  224x224   |
   |     0.91666     |     0.91512      | [ResNet101V2](https://drive.google.com/file/d/1jh8OaHg1DFLbZLJxygQeMvU1nVRlaHvb/view?usp=sharing) |          0.772 |          0.938 |  44,675,560 |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet101V2(public-0.91666%2C%20private-0.91512).ipynb) |      224x224       |  224x224   |
   |     0.89705     |     0.89647      | [ResNet152V2](https://drive.google.com/file/d/1yOFCoBasy-Gtn438QiPMA_BHs9JBtY9d/view?usp=sharing) |          0.780 |          0.942 |  60,380,648 |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet152V2(public-0.89705%2C%20private-0.89647).ipynb) |      224x224       |  224x224   |
   |     0.92156     |     0.91640      | [InceptionV3](https://drive.google.com/file/d/1aBePi6eqdXHpqnxMSRIyZZK3LrZy3kTo/view?usp=sharing) |          0.779 |          0.937 |  23,851,784 |  159  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionV3(public-0.92156%2C%20private-0.91640).ipynb) |      299x299       |  224x224   |
   |     0.94117     |     0.91408      | [InceptionResNetV2](https://drive.google.com/file/d/15PbssO2ZdUrtpL1iKTiCTGThM9rHxt7p/view?usp=sharing) |          0.803 |          0.953 |  55,873,736 |  572  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionResNetV2(public-0.94117%2C%20private-0.91408).ipynb) |      299x299       |  224x224   |
   |     0.81862     |     0.82831      | [InceptionV3](https://drive.google.com/file/d/1_oc49fIH1YZP4-8bzM9KwmdSWjP6nnYL/view?usp=sharing) |          0.779 |          0.937 |  23,851,784 |  159  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionV3(Input%20shape-299x299%2C%20public-0.81862%2C%20private-0.82831).ipynb) |      299x299       |  299x299   |
   |     0.73039     |     0.74758      | [InceptionResNetV2](https://drive.google.com/file/d/1zhL8x-GgwapwlTJ2g2KtjdZKkZ0Ey1CQ/view?usp=sharing) |          0.803 |          0.953 |  55,873,736 |  572  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionResNetV2(Input%20shape%20299x299%2C%20public-0.73039%2C%20private-0.74758).ipynb) |      299x299       |  299x299   |
   |     0.93137     |     0.91689      | [DenseNet121](https://drive.google.com/file/d/1RTz47GS80clxxCi7y8G__Pd4KGbKgzia/view?usp=sharing) |          0.750 |          0.923 |   8,062,504 |  121  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_DenseNet121(public-0.93137%2C%20private-0.91689).ipynb) |      224x224       |  224x224   |
   |     0.92156     |     0.91285      | [DenseNet169](https://drive.google.com/file/d/15kS7_mohTv6xVvr84Of1-H36ql0CjnLe/view?usp=sharing) |          0.762 |          0.932 |  14,307,880 |  169  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_DenseNet169(public-0.92156%2C%20private-0.91285).ipynb) |      224x224       |  224x224   |
   |     0.91666     |     0.90940      | [DenseNet201](https://drive.google.com/file/d/1A-tkg-SWwoN1WvGicnEH_nPTWr764wzC/view?usp=sharing) |          0.773 |          0.936 |  20,242,984 |  201  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_DenseNet201(public-0.91666%2C%20private-0.90940).ipynb) |      224x224       |  224x224   |
   |     0.94117     |     0.91862      | [Xception](https://drive.google.com/file/d/1aG1lzKJi6g_Nr9UDsbXSl6I6OaBKL2SS/view?usp=sharing) |          0.790 |          0.945 |  22,910,480 |  126  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_Xception(public-0.94117%2C%20private-0.91862).ipynb) |      299x299       |  224x224   |
   |     0.93137     |     0.91009      | [Xception](https://drive.google.com/file/d/1I0z8oP-n7ivUQHMyLD7AweJIL5jkXdmO/view?usp=sharing) |          0.790 |          0.945 |  22,910,480 |  126  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_Xception(Input%20shape-299x299%2C%20public-0.93137%2C%20private-0.91009).ipynb) |      299x299       |  299x299   |
   |     0.91666     |     0.89830      | [EfficientNetB0](https://drive.google.com/file/d/1snbUDflnqcxwPeJpYa0_1WjBu2F6oL_a/view?usp=sharing) |          0.763 |          0.932 |        5.3M |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB0(public-0.91666%2C%20private-0.89830).ipynb) |      224x224       |  224x224   |
   |     0.90686     |     0.90032      | [EfficientNetB1](https://drive.google.com/file/d/1Cla_OLv0V4xY7WfCevEZqw4UMGidJOGA/view?usp=sharing) |          0.788 |          0.944 |        7.8M |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB1(public-0.90686%2C%20private-0.90032).ipynb) |      240x240       |  224x224   |
   |     0.92647     |     0.90540      | [EfficientNetB1](https://drive.google.com/file/d/15iwJlkM2Cql7-COytXjrjKTMzBpyouxc/view?usp=sharing) |          0.788 |          0.944 |        7.8M |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB1(Input%20shape-240x240%2C%20public-0.92647%2C%20private-0.90540).ipynb) |      240x240       |  240x240   |
   |     0.74019     |     0.72913      | [EfficientNetB2](https://drive.google.com/file/d/1dfaBbZsj82JtoPK_V1I_ablnsWVjqD_b/view?usp=sharing) |          0.798 |          0.949 |        9.2M |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB2(public-0.74019%2C%20private-0.72913).ipynb) |      260x260       |  224x224   |
   |     0.94117     |     0.90930      | [EfficientNetB2](https://drive.google.com/file/d/1MyI1gfLV--7CaNleNsdDnxk39R7HR3Uw/view?usp=sharing) |          0.798 |          0.949 |        9.2M |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB2(Input%20shape-260x260%2C%20public-0.94117%2C%20private-0.90930).ipynb) |      260x260       |  260x260   |
   |     0.92156     |     0.90185      | [EfficientNetB3](https://drive.google.com/file/d/141J4nu1b7wbGTK0V3manhQ5x4TEx3rcu/view?usp=sharing) |          0.811 |          0.955 |         12M |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB3(public-0.92156%2C%20private-0.90185).ipynb) |      300x300       |  224x224   |
   |     0.92156     |     0.90693      | [EfficientNetB3](https://drive.google.com/file/d/1zAPOJHFmBBg2eswFCxIojfM_GD7RCoxu/view?usp=sharing) |          0.811 |          0.955 |         12M |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB3(Input%20shape-300x300%2C%20public-0.92156%2C%20private-0.90693).ipynb) |      300x300       |  300x300   |
   |     0.91176     |     0.91196      | [EfficientNetB4](https://drive.google.com/file/d/1T1tYaZTbvZg0GgVH4zAkpfHCgwLQ1Kqg/view?usp=sharing) |          0.826 |          0.963 |         19M |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB4(public-0.91176%2C%20private-0.91196).ipynb) |      380x380       |  224x224   |
   |        X        |        X         |                                               EfficientNetB4 |          0.826 |          0.963 |         19M |   -   |                              -                               |      380x380       |  380x380   |
   |     0.93137     |     0.90338      | [EfficientNetB5](https://drive.google.com/file/d/1282glNU40sbL_k0PDdIMaHiBsIUpp0KC/view?usp=sharing) |          0.833 |          0.967 |         30M |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB5(public-0.93137%2C%20private-0.90338).ipynb) |      456x456       |  224x224   |
   |        X        |        X         |                                               EfficientNetB5 |          0.833 |          0.967 |         30M |   -   |                              -                               |      456x456       |  456x456   |
   |     0.95588     |     0.91122      | [EfficientNetB6](https://drive.google.com/file/d/1jxP49w8mqZKwO1bpapf1VpQ7JV_jMyqd/view?usp=sharing) |          0.840 |          0.969 |         43M |   -   | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB6(public-0.95588%2C%20private-0.91122).ipynb) |      528x528       |  224x224   |
   |        X        |        X         |                                               EfficientNetB6 |          0.840 |          0.969 |         43M |   -   |                              -                               |      528x528       |  528x528   |
   |        X        |        X         |                                               EfficientNetB7 |          0.844 |          0.971 |         66M |   -   |                              -                               |      600x600       |  224x224   |
   |        X        |        X         |                                               EfficientNetB7 |          0.844 |          0.971 |         66M |   -   |                              -                               |      600x600       |  600x600   |

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
