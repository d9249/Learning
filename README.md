# JSP_Pic Project (producer: mean_ideal)
#### Git address: https://github.com/d9249/JSP_Pic  
#### deploy address: http://ec2-3-15-220-222.us-east-2.compute.amazonaws.com:8080/BookMarket/books.jsp?language=ko
### 프로젝트 계획 이유
> 항상 사람들이 잘 찍은 사진을 보면서, 저러한 사진은 어디서 어떻게 찍은 것일까?  
라는 호기심이 생겨 해당 정보를 알고싶지만, 직접 물어보기 어려운 상황이거나,  
연락을 취하기 곤란한 상황일 경우 정보를 얻기 어려울 수 있어 이러한 문제점을 극복하기 위해서  
Pic Board로 손쉽게 해당 사진에 대한 정보를 얻기위해서 해당 프로젝트를 설계하였습니다.

### 서비스 설명: 상단 Menu Bar  
##### [PIC]: 해당 사이트의 컨셉의 설명을 위한 메인 페이지
##### [LOGIN]: 해당 사이트의 서비스를 이용하기 위한 로그인 Page로 이동  
> Login에 성공할 경우: Login sucess page로 이동  
>>사이트 로그인 가능한 Id & Password  
>>Id: guest  
>>Password: guest1234  

##### 로그인에 성공할 경우 Menu Bar 변경사항  
> [LOGOUT]: 해당 사이트의 session을 잃으면서, logout을 실행.  
> [MY PAGE]: 회원가입한 자신의 정보를 수정, 탈퇴 할 수 있는 Page.

##### [SIGN UP]: 해당 사이트의 회원 가입을 할 수 있는 Page로 이동
> 유효성 검사에 통과하여 회원가입을 성공할 경우에 회원가입을 축하하는 Page로 이동

##### [PIC BOARD]
> 해당 사이트의 등록된 모든 게시글들을 볼 수 있는 공간.  

1. 우측 상단의 Korean & English를 통해 2가지 언어로 해당 사이트 이용 가능.  
2. 좌측 하단의 검색을 통해 제목, 설명, 글쓴이에 포함된 키워드에 대해 검색 기능.  
3. 로그인 후 우측 하단을 통해, 자신의 Pic에 대한 글을 적을 수 있는 Page로 이동.

##### [PIC BOARD] 유의사항
> 해당 게시글은 자신의 아이디를 통해서 로그인할 경우에만 수정, 삭제가 가능하며,  
다른 아이디로의 접속을 통해서는 수정, 삭제가 불가능합니다.
