**Paper**

1. **한글제목** : 알파벳으로 덮어씌워진 숫자 인식을 위한 기존 모델의 정확도 분석.
   **영문제목** :  

2. **국문요약** : 
   **영문요약** : 

3. **key words** : Overwritten number, convolution neural network, Image Classification.

4. **서론**

5. 1. **해당 논문을 작성하게 된 이유**
       Image Classification on MNIST의 정확도가 Top Accuracy 99.870를 달성한 모델의 성능을 보고 사람이 작성한 숫자를 분류해내는 정확도가 ILSVRC 대회 역대 우승 알고리즘들과 인식 에러율에서 사람의 숫자 분류 에러율을 5%로 보았을 때 사람보다 약 4.8% 더 높은 정확도를 보이는 것을 알 수 있었다. 앞서 설명한 숫자 분류의 문제는 단순히 필기체를 인식해 분류해내는 것에서 그쳤지만, DACON의 컴퓨터 비전 학습 경진 대회에서는 알파벳으로 가린 숫자의 일부분의 영역을 보아 감추어진 숫자를 예측하는 단순히 인식하여 예측하는 문제보다 더 Task가 높다고 볼 수 있다. 때문에 해당 문제에서도 딥러닝의 정확도가 어떠할지 분석하기 위한 과정이다. 

6. **관련연구**

7. 1. Image Classification. 
   2. Cursive recognition.

8. **제안연구** :

9. **실험결과 및 분석** :

10. **결론**

11. **참고문헌**

    ​		Image Classification on MNIST

    1. https://paperswithcode.com/sota/image-classification-on-mnist?metric=Accuracy

       Computer vision large-scale visual recognition challenge (ILSVRC)

    2. https://image-net.org/challenges/LSVRC/index.php

       ILSVRC 대회 역대 우승 알고리즘들과 인식 에러율

    3. https://bskyvision.com/425



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
