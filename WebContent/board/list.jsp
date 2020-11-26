<%@ page contentType="text/html; charset=utf-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<%@ page import="java.util.*"%>
<%@ page import="mvc.model.BoardDTO"%>
<%
	String sessionId = (String) session.getAttribute("sessionId");
	List boardList = (List) request.getAttribute("boardlist");
	int total_record = ((Integer) request.getAttribute("total_record")).intValue();
	int pageNum = ((Integer) request.getAttribute("pageNum")).intValue();
	int total_page = ((Integer) request.getAttribute("total_page")).intValue();
%>
<html>
<head>
<link rel="stylesheet" href="./resources/css/bootstrap.min.css" />
<meta name="viewport" content="width=device-width,initial-scale=1">
<title>Board</title>
<script type="text/javascript">
	function checkForm() {	
		if (${sessionId==null}) {
			alert("로그인 해주세요.");
			return false;
		}
		location.href = "./BoardWriteForm.do?id=<%=sessionId%>"
	}
</script>
<style>
	#nill {
		display: grid; grid-template-rows: repeat(2, 450px); grid-template-columns: repeat(2, 1fr); gap: 20px 10px;
	}
	
	@media screen and (max-width: 1000px) { 
		#nill { 
			display: grid; 
			grid-template-rows: repeat(4, 450px); 
			grid-template-columns: repeat(1, 1fr); 
			gap: 20px 10px; 
		} 
	}
</style>
</head>
<body>
	<jsp:include page="../menu.jsp" />
	<div class="jumbotron" style="background-color: #FFFFFF">
		<div class="container">
			<h1 class="display-3" align="right">
				<p style="font-family: 'Nanum Myeongjo', sans-serif;">
					<b>당신의 소중한 기억,</b>
				</p>
			</h1>
			<h2 class="display-4" align="right">
				<p style="font-family: 'Nanum Myeongjo', sans-serif;">공유 해주세요.</p>
			</h2>
		</div>
	</div>
	<div class="container">
		<form action="<c:url value="./BoardListAction.do"/>" method="post">
			<div>
				<div class="text-right">
					<span class="badge badge-success">전체 게시물: <%=total_record%>건
					</span>
				</div>
			</div>
			<div id="ni1" style="padding-top: 50px">
				<div class="container" id="nill">
					<%
						for (int j = 0; j < boardList.size(); j++) {
						BoardDTO notice = (BoardDTO) boardList.get(j);
					%>
					<div align="center">
						<a href="./BoardViewAction.do?num=<%=notice.getNum()%>&pageNum=<%=pageNum%>">
						<img src="./resources/images/<%=notice.getFilename() %>" style="height: 300px; margin-right: 40px"></a>
						<h5>
							<br>
							<b>제목: <%=notice.getSubject()%></b>
						</h5>
						<h5>
							<b>설명: <%=notice.getDescription()%></b>
						</h5>
						<h5>
							<b>작성자: <%=notice.getName()%></b>
						</h5>
						<h5>
							<b>조회수: <%=notice.getHit()%></b>
						</h5>
					</div>
					<%
						}
					%>
				</div>
			</div>
			<div align="center">
				<c:set var="pageNum" value="<%=pageNum%>" />
				<c:forEach var="i" begin="1" end="<%=total_page%>">
					<a href="<c:url value="./BoardListAction.do?pageNum=${i}" /> ">
						<c:choose>
							<c:when test="${pageNum==i}">
								<font color='4C5317'><b> [${i}]</b></font>
							</c:when>
							<c:otherwise>
								<font color='4C5317'> [${i}]</font>
							</c:otherwise>
						</c:choose>
					</a>
				</c:forEach>
			</div>
			<div align="left">
				<table>
					<tr>
						<td width="100%" align="left" style="width:100%; height:100px;">&nbsp;&nbsp; 
							<select	name="items" class="txt" style="height: 40px;">
								<option value="subject">제목에서</option>
								<option value="description">설명에서</option>
								<option value="name">글쓴이에서</option>
							</select>
							<input name="text" type="text" style="height: 40px;"/> 
							<input type="submit" id="btnAdd" class="btn btn-primary" style="height: 43.5px;" value="검색"/>
						</td>
						<td width="100%" align="right">
							<a href="#"	onclick="checkForm(); return false;" class="btn btn-primary"
								role="button" style="width: 120px; height:43.5px; margin-left: 10px; padding-top:8px;">&laquo; 함께하기</a>
						</td>
					</tr>
				</table>
			</div>
		</form>
		<hr>
	</div>
	<jsp:include page="../footer.jsp" />
</body>
</html>





