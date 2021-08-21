## Voting ensemble
> Found 1642 images belonging to 10 classes.
>
> Found 406 images belonging to 10 classes.
>
> Train data set 2048개를 1642개를 train data로 406개를 validation data로 나누어서 학습을 진행하였으며,
>
> ImageDataGenerator을 사용하여서 Data Argmentation을 진행한 결과 학습에는
>
> Train image = 52544, Validataion image = 12992가 사용되었습니다. 
>
> 11개, 25개의 모델의 비교를 진행할 때 VGG16, VGG19 2개의 model running error가 있어서 학습을 중단하였으며,
>
> Individual model(Before parameter optimization)을 통해서 개별적으로 학습된 모델을 이용하여서 Voting ensemble 하였다.

### Voting ensemble - 11 models
| val_accuracy |             Model | Top-1 accuracy | Top-5 accuracy |   Parameter | Depth |
| :----------: | ----------------: | -------------: | -------------: | ----------: | :---: |
|      X       |             VGG16 |          0.713 |          0.901 | 138,357,544 |  23   |
|      X       |             VGG19 |          0.713 |          0.900 | 143,667,240 |  26   |
|   0.92857    |          ResNet50 |          0.749 |          0.921 |  25,636,712 |   -   |
|   0.93596    |         ResNet101 |          0.764 |          0.928 |  44,707,176 |   -   |
|   0.92857    |         ResNet152 |          0.766 |          0.931 |  60,419,944 |   -   |
|   0.93350    |        ResNet50V2 |          0.760 |          0.930 |  25,613,800 |   -   |
|   0.92118    |       ResNet101V2 |          0.772 |          0.938 |  44,675,560 |   -   |
|   0.93596    |       ResNet152V2 |          0.780 |          0.942 |  60,380,648 |   -   |
|   0.94581    |       InceptionV3 |          0.779 |          0.937 |  23,851,784 |  159  |
|   0.93103    | InceptionResNetV2 |          0.803 |          0.953 |  55,873,736 |  572  |
|   0.93842    |       DenseNet121 |          0.750 |          0.923 |   8,062,504 |  121  |
|   0.93596    |       DenseNet169 |          0.762 |          0.932 |  14,307,880 |  169  |
|   0.93103    |       DenseNet201 |          0.773 |          0.936 |  20,242,984 |  201  |

> 11개의 model을 ensemble 한 결과 - public-0.94607, private-0.93317로 private의 성능 향상을 보였다. [Result Link.](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_07_Eleven_model_ensemble(public-0.94607%2C%20private-0.93317).ipynb)

![image](https://user-images.githubusercontent.com/60354713/128587281-2a2cf2be-b2ee-4139-aaf5-c715bebb2744.png)

### Voting ensemble - 25 models

| Model                                                        | Private acc | Result Link                                                  | Default input size | Input size |
| ------------------------------------------------------------ | ----------- | ------------------------------------------------------------ | ------------------ | ---------- |
| VGG16                                                        | X           |                                                              | 224x224            | 224x224    |
| VGG19                                                        | X           |                                                              | 224x224            | 224x224    |
| [ResNet50](https://drive.google.com/file/d/1wPau-IXTl15NRIDe6xSjcMNrYT3jGP8R/view?usp=sharing) | 0.90816     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet50(public-0.92156%2C%20private-0.90816).ipynb) | 224x224            | 224x224    |
| [ResNet101](https://drive.google.com/file/d/1L98_1aydEzZuRPfl7CYadLJ9I6K_eM55/view?usp=sharing) | 0.90377     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet101(public-0.92857%2C%20private-0.90377).ipynb) | 224x224            | 224x224    |
| [ResNet152](https://drive.google.com/file/d/1PSrJRKc4dd8R2FGhifNOku3JkYsl-Ikb/view?usp=sharing) | 0.89568     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet152(public-0.90196%2C%20private-0.89568).ipynb) | 224x224            | 224x224    |
| [ResNet50V2](https://drive.google.com/file/d/1qv9lv15CQFarucihFBw0eI6BigjXu-gh/view?usp=sharing) | 0.90076     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet50V2(public-0.89215%2C%20private-0.90076).ipynb) | 224x224            | 224x224    |
| [ResNet101V2](https://drive.google.com/file/d/1jh8OaHg1DFLbZLJxygQeMvU1nVRlaHvb/view?usp=sharing) | 0.91512     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet101V2(public-0.91666%2C%20private-0.91512).ipynb) | 224x224            | 224x224    |
| [ResNet152V2](https://drive.google.com/file/d/1yOFCoBasy-Gtn438QiPMA_BHs9JBtY9d/view?usp=sharing) | 0.89647     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet152V2(public-0.89705%2C%20private-0.89647).ipynb) | 224x224            | 224x224    |
| [InceptionV3](https://drive.google.com/file/d/1aBePi6eqdXHpqnxMSRIyZZK3LrZy3kTo/view?usp=sharing) | 0.91640     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionV3(public-0.92156%2C%20private-0.91640).ipynb) | 299x299            | 224x224    |
| [InceptionResNetV2](https://drive.google.com/file/d/15PbssO2ZdUrtpL1iKTiCTGThM9rHxt7p/view?usp=sharing) | 0.91408     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionResNetV2(public-0.94117%2C%20private-0.91408).ipynb) | 299x299            | 224x224    |
| [InceptionV3](https://drive.google.com/file/d/1_oc49fIH1YZP4-8bzM9KwmdSWjP6nnYL/view?usp=sharing) | 0.82831     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionV3(Input%20shape-299x299%2C%20public-0.81862%2C%20private-0.82831).ipynb) | 299x299            | 299x299    |
| [InceptionResNetV2](https://drive.google.com/file/d/1zhL8x-GgwapwlTJ2g2KtjdZKkZ0Ey1CQ/view?usp=sharing) | 0.74758     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionResNetV2(Input%20shape%20299x299%2C%20public-0.73039%2C%20private-0.74758).ipynb) | 299x299            | 299x299    |
| [DenseNet121](https://drive.google.com/file/d/1RTz47GS80clxxCi7y8G__Pd4KGbKgzia/view?usp=sharing) | 0.91689     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_DenseNet121(public-0.93137%2C%20private-0.91689).ipynb) | 224x224            | 224x224    |
| [DenseNet169](https://drive.google.com/file/d/15kS7_mohTv6xVvr84Of1-H36ql0CjnLe/view?usp=sharing) | 0.91285     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_DenseNet169(public-0.92156%2C%20private-0.91285).ipynb) | 224x224            | 224x224    |
| [DenseNet201](https://drive.google.com/file/d/1A-tkg-SWwoN1WvGicnEH_nPTWr764wzC/view?usp=sharing) | 0.90940     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_DenseNet201(public-0.91666%2C%20private-0.90940).ipynb) | 224x224            | 224x224    |
| [Xception](https://drive.google.com/file/d/1aG1lzKJi6g_Nr9UDsbXSl6I6OaBKL2SS/view?usp=sharing) | 0.91862     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_Xception(public-0.94117%2C%20private-0.91862).ipynb) | 299x299            | 224x224    |
| [Xception](https://drive.google.com/file/d/1I0z8oP-n7ivUQHMyLD7AweJIL5jkXdmO/view?usp=sharing) | 0.91009     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_Xception(Input%20shape-299x299%2C%20public-0.93137%2C%20private-0.91009).ipynb) | 299x299            | 299x299    |
| [EfficientNetB0](https://drive.google.com/file/d/1snbUDflnqcxwPeJpYa0_1WjBu2F6oL_a/view?usp=sharing) | 0.89830     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB0(public-0.91666%2C%20private-0.89830).ipynb) | 224x224            | 224x224    |
| [EfficientNetB1](https://drive.google.com/file/d/1Cla_OLv0V4xY7WfCevEZqw4UMGidJOGA/view?usp=sharing) | 0.90032     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB1(public-0.90686%2C%20private-0.90032).ipynb) | 240x240            | 224x224    |
| [EfficientNetB1](https://drive.google.com/file/d/15iwJlkM2Cql7-COytXjrjKTMzBpyouxc/view?usp=sharing) | 0.90540     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB1(Input%20shape-240x240%2C%20public-0.92647%2C%20private-0.90540).ipynb) | 240x240            | 240x240    |
| [EfficientNetB2](https://drive.google.com/file/d/1dfaBbZsj82JtoPK_V1I_ablnsWVjqD_b/view?usp=sharing) | 0.72913     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB2(public-0.74019%2C%20private-0.72913).ipynb) | 260x260            | 224x224    |
| [EfficientNetB2](https://drive.google.com/file/d/1MyI1gfLV--7CaNleNsdDnxk39R7HR3Uw/view?usp=sharing) | 0.90930     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB2(Input%20shape-260x260%2C%20public-0.94117%2C%20private-0.90930).ipynb) | 260x260            | 260x260    |
| [EfficientNetB3](https://drive.google.com/file/d/141J4nu1b7wbGTK0V3manhQ5x4TEx3rcu/view?usp=sharing) | 0.90185     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB3(public-0.92156%2C%20private-0.90185).ipynb) | 300x300            | 224x224    |
| [EfficientNetB3](https://drive.google.com/file/d/1zAPOJHFmBBg2eswFCxIojfM_GD7RCoxu/view?usp=sharing) | 0.90693     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB3(Input%20shape-300x300%2C%20public-0.92156%2C%20private-0.90693).ipynb) | 300x300            | 300x300    |
| [EfficientNetB4](https://drive.google.com/file/d/1T1tYaZTbvZg0GgVH4zAkpfHCgwLQ1Kqg/view?usp=sharing) | 0.91196     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB4(public-0.91176%2C%20private-0.91196).ipynb) | 380x380            | 224x224    |
| EfficientNetB4                                               | X           |                                                              | 380x380            | 380x380    |
| [EfficientNetB5](https://drive.google.com/file/d/1282glNU40sbL_k0PDdIMaHiBsIUpp0KC/view?usp=sharing) | 0.90338     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB5(public-0.93137%2C%20private-0.90338).ipynb) | 456x456            | 224x224    |
| EfficientNetB5                                               | X           |                                                              | 456x456            | 456x456    |
| [EfficientNetB6](https://drive.google.com/file/d/1jxP49w8mqZKwO1bpapf1VpQ7JV_jMyqd/view?usp=sharing) | 0.91122     | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_EfficientNetB6(public-0.95588%2C%20private-0.91122).ipynb) | 528x528            | 224x224    |
| EfficientNetB6                                               | X           |                                                              | 528x528            | 528x528    |
| EfficientNetB7                                               | X           |                                                              | 600x600            | 224x224    |
| EfficientNetB7                                               | X           |                                                              | 600x600            | 600x600    |

> 25개의 model을 voting ensemble 한 결과 - public-0.94117, private-0.93386로 private의 성능 향상을 보였다. [Result Link.](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_07_Twenty-five%20model%20voting%20ensemble(public-0.94117%2C%20private-0.93386).ipynb)
>
> 추가적으로 ImageDataGenerator를 통해서 데이터 증강을 더욱 해당 문제에 최적화하는 방법과, Optimizer를 바꾸어서 학습한 경우 더 높은 성능을 보일 수 있을것같다.

![image](https://user-images.githubusercontent.com/60354713/129487841-90229ce0-62e0-4f81-8a17-389e28470451.png)

