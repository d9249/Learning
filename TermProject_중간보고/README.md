# 무릎  X-ray 영상을  이용한  KL grade Classifier

### 관련 논문

1. Automatic Grading of Individual Knee Osteoarthritis Features in Plain Radiographs Using Deep Convolutional Neural Networks
2. Attention-based CNN for KL Grade Classification: Data from the Osteoarthritis Initiative
3. Automated Classification of Radiographic Knee Osteoarthritis Severity Using Deep Neural Networks

### Some of Available Datasets

1. https://nda.nih.gov/oai/

### Classification

![KL grade explanation](https://github.com/d9249/MDL/blob/main/TermProject/KL-grade%20explanation.png)

![KL grade example](https://github.com/d9249/MDL/blob/main/TermProject/KL-grade%20example.png)

Numerous variations of the Kellgren and Lawrence classification system have been used in research 3. Below is the original description 1-3:

grade 0 (none): definite absence of x-ray changes of osteoarthritis

grade 1 (doubtful): doubtful joint space narrowing and possible osteophytic lipping

grade 2 (minimal): definite osteophytes and possible joint space narrowing

grade 3 (moderate): moderate multiple osteophytes, definite narrowing of joint space and some sclerosis and possible deformity of bone ends

grade 4 (severe): large osteophytes, marked narrowing of joint space, severe sclerosis and definite deformity of bone ends

Osteoarthritis is deemed present at grade 2 although of minimal severity 1.

단순 방사선 사진이 가장 유용하다. 

초기에는 정상 소견을 보일 수 있으나 점진적으로 관절 간격의 감소가 나타나며 연골 아래 뼈의 음영이 짙어지는 경화 소견을 볼 수 있다. 

더욱 진행되면 관절면의 가장 자리에 뼈가 웃자란 듯한 골극이 형성되고 관절면이 불규칙해진다. 

이차성 관절염의 경우 원인이 되는 과거 외상이나 질환의 흔적 혹은 변형 등이 관찰되기도 한다. 

다만 방사선학적 변화가 증상 및 활동력의 심한 정도를 그대로 반영하는 것은 아니어서 40세 이상에서 90% 정도는 방사선학적으로 퇴행성 변화를 보이지만 이 중 30% 정도만이 증상을 보이게 된다.

http://www.snuh.org/health/nMedInfo/nView.do?category=DIS&medid=AA000196



### idea

1. https://kmhana.tistory.com/27
   
   ![img](https://blog.kakaocdn.net/dn/um3iU/btq8R0EvnO7/foZAXe9cpW7ycFzL9K5lO1/img.png)
   
   Attention을 활용하여서 무릎 관절 사이의 정보를 주의 깊게 활용.
   
   https://arxiv.org/abs/1706.03762
   
   https://glee1228.tistory.com/3

2. Pycaret을 통한 모델 최적화

### Ref

https://www.kaggle.com/akashkewar/data-sprint-35-osteoarthritis-kneexray

https://github.com/PingjunChen/GradingKneeOA

https://www.kaggle.com/gpiosenka/notebookf03b3b1161

### A competition site for accuracy estimation

https://dphi.tech/challenges/data-sprint-35-osteoarthritis-knee-x-ray/81/overview/about
