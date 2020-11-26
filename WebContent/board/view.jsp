<%@ page contentType="text/html; charset=utf-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>

<%@ page import="mvc.model.BoardDTO"%>

<%
	BoardDTO notice = (BoardDTO) request.getAttribute("board");
	int num = ((Integer) request.getAttribute("num")).intValue();
	int nowpage = ((Integer) request.getAttribute("page")).intValue();
%>
<html>
<head>
<link rel="stylesheet" href="./resources/css/bootstrap.min.css" />
<title>Board</title>
</head>
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
		<form name="newUpdate"
			action="BoardUpdateAction.do?num=<%=notice.getNum()%>&pageNum=<%=nowpage%>"
			class="form-horizontal" method="post">
			
			<div class="form-group row">
 				<img src="./resources/images/<%=notice.getFilename() %>" style="width:100%;">
 			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >성명</label>
				<div class="col-sm-5">
					<input name="name" class="form-control"	value=" <%=notice.getName()%>">
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >제목</label>
				<div class="col-sm-5">
					<input name="subject" class="form-control"	value=" <%=notice.getSubject()%>" >
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >촬영한 장소</label>
				<div class="col-sm-5">
					<input name="address" class="form-control"	value=" <%=notice.getAddress()%>" >
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >설명</label>
				<div class="col-sm-5" style="word-break: break-all;">
					<textarea name="description" class="form-control" cols="50" rows="2"> <%=notice.getDescription()%></textarea>
				</div>
			</div>
			
			<div class="form-group row">
				<label class="col-sm-2 control-label" >사용한 카메라</label>
				<div class="col-sm-5">
					<input name="camera" class="form-control" value=" <%=notice.getCamera()%>" >
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >사용한 필터</label>
				<div class="col-sm-5">
					<input name="filter" class="form-control" value=" <%=notice.getFilter()%>" >
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >촬영한 시간</label>
				<div class="col-sm-5">
					<input name="photoTime" class="form-control" value=" <%=notice.getPhotoTime()%>" >
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >분류</label>
				<div class="col-sm-5">
					<input name="category" class="form-control"	value=" <%=notice.getCategory()%>" >
				</div>
			</div>
			<div class="form-group row">
				<label class="col-sm-2 control-label" >이미지</label>
				<div class="col-sm-5">
					<input name="filename" type="file" class="form-control"	value=" <%=notice.getFilename()%>" >
				</div>
			</div>
			<div class="form-group row">
				<div class="col-sm-offset-2 col-sm-10">
					<c:set var="userId" value="<%=notice.getId()%>" />
					<c:if test="${sessionId==userId}">
						<p><a href="./BoardDeleteAction.do?num=<%=notice.getNum()%>&pageNum=<%=nowpage%>" class="btn btn-danger"> 삭제</a> 
						<input type="submit" class="btn btn-success" value="수정">
					</c:if>
					<a href="./BoardListAction.do?pageNum=<%=nowpage%>" class="btn btn-primary"> 목록</a>
				</div>
			</div>
		</form>
		<hr>
	</div>
	<jsp:include page="../footer.jsp" />
</body>
</html>


