<%@ page contentType="text/html; charset=utf-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<%@ taglib prefix="fmt" uri="http://java.sun.com/jsp/jstl/fmt"%>

<html>
<head>
<link rel="stylesheet" href="../resources/css/bootstrap.min.css" />
<link rel="stylesheet" href="../resources/css/bootstrap.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
<link href="https://fonts.googleapis.com/css?family=Poppins:100,200,300,400,500,600,700,800,900" rel="stylesheet">
<link href="https://fonts.googleapis.com/css?family=Abril+Fatface&display=swap" rel="stylesheet">
<link rel="stylesheet" href="../resources/css/open-iconic-bootstrap.min.css">
<link rel="stylesheet" href="../resources/css/animate.css">
<link rel="stylesheet" href="../resources/css/owl.carousel.min.css">
<link rel="stylesheet" href="../resources/css/owl.theme.default.min.css">
<link rel="stylesheet" href="../resources/css/magnific-popup.css">
<link rel="stylesheet" href="../resources/css/aos.css">
<link rel="stylesheet" href="../resources/css/ionicons.min.css">
<link rel="stylesheet" href="../resources/css/bootstrap-datepicker.css">
<link rel="stylesheet" href="../resources/css/jquery.timepicker.css">
<link rel="stylesheet" href="../resources/css/flaticon.css">
<link rel="stylesheet" href="../resources/css/icomoon.css">
<link rel="stylesheet" href="../resources/css/style.css">
c<script type="text/javascript">
	function checkForm() {

		var regExpId = /^[a-z|A-Z|ㄱ-ㅎ|ㅏ-ㅣ|가-힣]/;
		var regExpName = /^[가-힣]*$/;

		var form = document.newMember;

		var id = form.id.value;
		var name = form.name.value;

		if (!regExpId.test(id)) {
			alert("아이디는 문자로 시작해 주세요!");
			form.id.select();
			return false;
		}
		if (!regExpName.test(name)) {
			alert("이름은 한글만으로 입력해 주세요!");
			return false;
		}
				
		if (!document.newMember.id.value) {
			alert("아이디를 입력하세요.");
			return false;
		}

		if (!document.newMember.password.value) {
			alert("비밀번호를 입력하세요.");
			return false;
		}

		if (document.newMember.password.value != document.newMember.password_confirm.value) {
			alert("비밀번호를 동일하게 입력하세요.");
			return false;
		}
		if (!document.newMember.name.value) {
			alert("이름을 입력하세요.");
			return false;
		}
		if (!document.newMember.mail1.value) {
			alert("이메일을 입력하세요.");
			return false;
		}
		if (!document.newMember.mail2.value) {
			alert("이메일 도메인을 선택하세요.");
			return false;
		}
		if (!document.newMember.phone.value) {
			alert("전화번호를 입력하세요.");
			return false;
		}
		if (!document.newMember.address.value) {
			alert("주소를 입력하세요.");
			return false;
		}
		if (!document.newMember.birthyy.value) {
			alert("생년를 선택하세요.");
			return false;
		}
		if (!document.newMember.birthdd.value) {
			alert("생년 일을 선택하세요.");
			return false;
		}
		if (!document.newMember.birthmm.value) {
			alert("생년 월을 선택하세요.");
			return false;
		}
	}
</script>
<title>회원 가입</title>
</head>
<body>
	<%@ include file="/menu.jsp"%>
	<fmt:setLocale value='<%=request.getParameter("language")%>' />
	<fmt:setBundle basename="bundle.Translation"/>
	<fmt:bundle basename="bundle.message" />
	<div class="jumbotron" style="background-color: #FFFFFF">
		<div class="container">
			<h1 class="display-3" align="right">
				<p style="font-family: 'Nanum Myeongjo', sans-serif;">
					<b><fmt:message key="main1"/></b>
				</p>
			</h1>
			<h2 class="display-4" align="right">
				<p style="font-family: 'Nanum Myeongjo', sans-serif;"><fmt:message key="main2"/></p>
			</h2>
		</div>
	</div>
	<div class="container">
		<div>
			<a href="?language=ko">Korean</a> | <a href="?language=en">English</a>
		</div>
		<br>
		<form name="newMember" class="form-horizontal"  action="processAddMember.jsp" method="post" onsubmit="return checkForm()">
			<div class="form-group  row">
				<label class="col-sm-2 "><fmt:message key="Id"/></label>
				<div class="col-sm-3">
					<input name="id" type="text" class="form-control" placeholder="id" >
				</div>
			</div>
			<div class="form-group  row">
				<label class="col-sm-2"><fmt:message key="password"/></label>
				<div class="col-sm-3">
					<input name="password" type="text" class="form-control" placeholder="password" >
				</div>
			</div>
			<div class="form-group  row">
				<label class="col-sm-2"><fmt:message key="password_confirm"/></label>
				<div class="col-sm-3">
					<input name="password_confirm" type="text" class="form-control" placeholder="password confirm" >
				</div>
			</div>
			<div class="form-group  row">
				<label class="col-sm-2"><fmt:message key="name"/></label>
				<div class="col-sm-3">
					<input name="name" type="text" class="form-control" placeholder="name" >
				</div>
			</div>
			<div class="form-group  row">
				<label class="col-sm-2"><fmt:message key="gender"/></label>
				<div class="col-sm-10">
					<input name="gender" type="radio" value="남" /><fmt:message key="gender1"/> 
					<input name="gender" type="radio" value="여" /><fmt:message key="gender2"/>
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2"><fmt:message key="birthday"/></label>
				<div class="col-sm-4  ">
<!-- 					<input type="text" name="birthyy" maxlength="4" placeholder="년(4자)" size="6">
 -->				<select name="birthyy">
						<option value=""><fmt:message key="yy"/></option>
						<option value="1950">1950</option>
						<option value="1951">1951</option>
						<option value="1952">1952</option>
						<option value="1953">1953</option>
						<option value="1954">1954</option>
						<option value="1955">1955</option>
						<option value="1956">1956</option>
						<option value="1957">1957</option>
						<option value="1958">1958</option>
						<option value="1959">1959</option>
						<option value="1960">1960</option>
						<option value="1961">1961</option>
						<option value="1962">1962</option>
						<option value="1963">1963</option>
						<option value="1964">1964</option>
						<option value="1965">1965</option>
						<option value="1966">1966</option>
						<option value="1967">1967</option>
						<option value="1968">1968</option>
						<option value="1969">1969</option>
						<option value="1970">1970</option>
						<option value="1971">1971</option>
						<option value="1972">1972</option>
						<option value="1973">1973</option>
						<option value="1974">1974</option>
						<option value="1975">1975</option>
						<option value="1976">1976</option>
						<option value="1977">1977</option>
						<option value="1978">1978</option>
						<option value="1979">1979</option>
						<option value="1980">1980</option>
						<option value="1981">1981</option>
						<option value="1982">1982</option>
						<option value="1983">1983</option>
						<option value="1984">1984</option>
						<option value="1985">1985</option>
						<option value="1986">1986</option>
						<option value="1987">1987</option>
						<option value="1988">1988</option>
						<option value="1989">1989</option>
						<option value="1990">1990</option>
						<option value="1991">1991</option>
						<option value="1992">1992</option>
						<option value="1993">1993</option>
						<option value="1994">1994</option>
						<option value="1995">1995</option>
						<option value="1996">1996</option>
						<option value="1997">1997</option>
						<option value="1998">1998</option>
						<option value="1999">1999</option>
						<option value="2000">2000</option>
						<option value="2001">2001</option>
						<option value="2002">2002</option>
						<option value="2003">2003</option>
						<option value="2004">2004</option>
						<option value="2005">2005</option>
						<option value="2006">2006</option>
						<option value="2007">2007</option>
						<option value="2008">2008</option>
						<option value="2009">2009</option>
						<option value="2010">2010</option>
						<option value="2011">2011</option>
						<option value="2012">2012</option>
						<option value="2013">2013</option>
						<option value="2014">2014</option>
						<option value="2015">2015</option>
						<option value="2016">2016</option>
						<option value="2017">2017</option>
						<option value="2018">2018</option>
						<option value="2019">2019</option>
						<option value="2020">2020</option>												
					</select>
					<select name="birthmm">
						<option value=""><fmt:message key="mm"/></option>
						<option value="01">1</option>
						<option value="02">2</option>
						<option value="03">3</option>
						<option value="04">4</option>
						<option value="05">5</option>
						<option value="06">6</option>
						<option value="07">7</option>
						<option value="08">8</option>
						<option value="09">9</option>
						<option value="10">10</option>
						<option value="11">11</option>
						<option value="12">12</option>
					</select>
					<select name="birthdd">
						<option value=""><fmt:message key="dd"/></option>
						<option value="01">1</option>
						<option value="02">2</option>
						<option value="03">3</option>
						<option value="04">4</option>
						<option value="05">5</option>
						<option value="06">6</option>
						<option value="07">7</option>
						<option value="08">8</option>
						<option value="09">9</option>
						<option value="10">10</option>
						<option value="11">11</option>
						<option value="12">12</option>
						<option value="13">13</option>
						<option value="14">14</option>
						<option value="15">15</option>
						<option value="16">16</option>
						<option value="17">17</option>
						<option value="18">18</option>
						<option value="19">19</option>
						<option value="20">20</option>
						<option value="21">21</option>
						<option value="22">22</option>
						<option value="23">23</option>
						<option value="24">24</option>
						<option value="25">25</option>
						<option value="26">26</option>
						<option value="27">27</option>
						<option value="28">28</option>
						<option value="29">29</option>
						<option value="30">30</option>
						<option value="31">31</option>
					</select>
<!-- 					<input type="text" name="birthdd" maxlength="2" placeholder="일" size="4"> -->				
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2"><fmt:message key="email"/></label>
				<div class="col-sm-10">
					<input type="text" name="mail1" maxlength="50"> @  
					<select name="mail2">					
						<option value="naver.com">naver.com</option>
						<option value="daum.net">daum.net</option>
						<option value="gmail.com">gmail.com</option>
						<option value="Kyinggi.ac.kr">Kyonggi.ac.kr</option>
						<option value="Korea.com">korea.com</option>					
					</select>
				</div>				
			</div>
			<div class="form-group row">
				<label class="col-sm-2"><fmt:message key="phone_number"/></label>
				<div class="col-sm-3">
					<input name="phone" type="text" class="form-control" placeholder="phone" >

				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 "><fmt:message key="address"/></label>
				<div class="col-sm-5">
					<input name="address" type="text" class="form-control" placeholder="address">

				</div>
			</div>
			<div class="form-group row">
				<div class="col-sm-offset-2 col-sm-10 ">
					<input type="submit" class="btn btn-primary " value="<fmt:message key="submit"/>" > 
					<input type="reset" class="btn btn-primary " value="<fmt:message key="cancel"/>" onclick="reset()" >
				</div>
			</div>
		</form>
	</div>
	<hr>
	<jsp:include page="../footer.jsp" />
</body>
</html>