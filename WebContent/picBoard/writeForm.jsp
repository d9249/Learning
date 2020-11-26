<%@ page contentType="text/html; charset=utf-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<%
	String name = (String) request.getAttribute("name");
	
%>
<html>
<head>
<link rel="stylesheet" href="./resources/css/bootstrap.min.css" />
<title>Pic Board</title>
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
	<div class="jumbotron">
		<div class="container">
			<h1 class="display-3">글 작성</h1>
		</div>
	</div>

	<div class="container">

		<form name="newWrite" action="./PicBoardWriteAction.do"
			class="form-horizontal" method="post" onsubmit="return checkForm()">
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
				<div class="col-sm-offset-2 col-sm-10 ">
				 <input type="submit" class="btn btn-primary " value="등록 ">				
					 <input type="reset" class="btn btn-primary " value="취소 ">
				</div>
			</div>
		</form>
		<hr>
	</div>
	<jsp:include page="../footer.jsp" />
</body>
</html>



