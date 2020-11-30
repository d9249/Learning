<%@ page contentType="text/html; charset=utf-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<%
	String name = (String) request.getAttribute("name");
%>
<html>
<head>
<link rel="stylesheet" href="./resources/css/bootstrap.min.css" />
<title>Board</title>
</head>
<script type="text/javascript">
	function checkForm() {
		if (!document.newWrite.name.value) {
			alert("성명을 입력하세요.");
			return false;
		}
		if (!document.newWrite.subject.value) {
			alert("제목을 입력하세요.");
			return false;
		}
		if (!document.newWrite.address.value) {
			alert("장소를 입력하세요.");
			return false;
		}
		if (!document.newWrite.description.value) {
			alert("설명을 입력하세요.");
			return false;
		}
		if (!document.newWrite.camera.value) {
			alert("사용한 카메라를 입력하세요.");
			return false;
		}
		if (!document.newWrite.filter.value) {
			alert("사용한 필터를 입력하세요.");
			return false;
		}
		if (!document.newWrite.photoTime.value) {
			alert("촬영한 시간을 입력하세요.");
			return false;
		}
		if (!document.newWrite.category.value) {
			alert("분류을 입력하세요.");
			return false;
		}
	}
</script>
<body>
	<jsp:include page="../menu.jsp" />
	<div class="jumbotron" style="background-color: #FFFFFF">
		<div class="container">
			<h1 class="display-3" align="right">
				<p style="font-family: 'Nanum Myeongjo', sans-serif;">
					<b>당신의 사진 속 기억,</b>
				</p>
			</h1>
			<h2 class="display-4" align="right">
				<p style="font-family: 'Nanum Myeongjo', sans-serif;">공유 해주세요.</p>
			</h2>
		</div>
	</div>

	<div class="container">

		<form name="newWrite" action="./BoardWriteAction.do"
			class="form-horizontal" method="post" enctype="multipart/form-data" onsubmit="return checkForm()">
			<input name="id" type="hidden" class="form-control"
				value="${sessionId}">
			<div class="form-group row">
				<label class="col-sm-2 control-label" >성명</label>
				<div class="col-sm-3">
					<input name="name" type="text" class="form-control" value="<%=name%>"
						placeholder="name">
				</div>
			</div> 
			<div class="form-group row">
				<label class="col-sm-2 control-label" >제목</label>
				<div class="col-sm-5">

					<input name="subject" type="text" class="form-control"
						placeholder="subject">
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >촬영한 장소</label>
				<div class="col-sm-8">
					<textarea name="address" cols="50" rows="2" class="form-control"
						placeholder="address"></textarea>
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >설명</label>
				<div class="col-sm-8">
					<textarea name="description" cols="50" rows="5" class="form-control"
						placeholder="description"></textarea>
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >사용한 카메라</label>
				<div class="col-sm-5">
					<input name="camera" type="text" class="form-control"
						placeholder="camera">
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >사용한 필터</label>
				<div class="col-sm-5">
					<input name="filter" type="text" class="form-control"
						placeholder="filter">
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >촬영한 시간</label>
				<div class="col-sm-5">
					<input name="photoTime" type="text" class="form-control"
						placeholder="photoTime">
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >분류</label>
				<div class="col-sm-5">
					<input name="category" type="text" class="form-control"
						placeholder="category">
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >파일 이름</label>
				<div class="col-sm-5">
					<input name="filename" type="file" class="form-control"
						placeholder="filename">
				</div>
			</div>
			<div>
				<div style="display: flex; justify-content: flex-end;">
					<input type="submit" class="btn btn-primary " value="등록 " style="margin-right:5px;">		
					<input type="reset" class="btn btn-primary " value="취소 ">
				</div>
			</div>
		</form>
		<hr>
	</div>
	<jsp:include page="../footer.jsp" />
</body>
</html>



