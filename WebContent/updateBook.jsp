<%@ page contentType="text/html; charset=UTF-8"%>
<%@ page import="java.sql.*" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link rel="stylesheet" href="./resources/css/bootstrap.min.css">
<title>상품 수정</title>
</head>
<body>
	<%@include file="menu.jsp" %>
	<div class = "jumbotron">
		<div class = "container">
			<h1 class = "display-3">
				책 정보 수정
			</h1>
		</div>
	</div>
	<%@include file="dbconn.jsp" %>
	<%
		String bookId = request.getParameter("id");
		
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		
		String sql = "select * from book where b_id =?";
		pstmt = conn.prepareStatement(sql);
		pstmt.setString(1, bookId);
		rs = pstmt.executeQuery();
		if (rs.next()) {
	%>
	<div class="container">
		<div class="row">
			<div class="col-md-5">
				<img src="./resources/images/<%=rs.getString("b_filename") %>" alt="image" style="width: 100%" />
			</div>
			<div class="col-md-7">
				<form name="newBook" action="./processUpdateBook.jsp" class="form-horizontal" method="post" enctype="multipart/form-data">
					<div class="form-group row">
						<label class="col-sm-2"> 상품 코드</label>
						<div class="col-sm-3">
							<input type="text" id="bookId" name="bookId" class="form-control" value='<%=rs.getString("b_id") %>'>
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 상품명</label>
						<div class="col-sm-3">
							<input type="text" id="name" name="name" class="form-control" value='<%=rs.getString("b_name") %>'>
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 가격</label>
						<div class="col-sm-3">
							<input type="text" id="unitPrice" name="unitPrice" class="form-control" value='<%=rs.getString("b_unitPrice") %>'>
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 상세 설명</label>
						<div class="col-sm-5">
							<input type="text" id="description" name="description" class="form-control" value='<%=rs.getString("b_description") %>'>
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 출판사</label>
						<div class="col-sm-3">
							<input type="text" id="publisher" name="publisher" class="form-control" value='<%=rs.getString("b_publisher") %>'>
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 저자</label>
						<div class="col-sm-3">
							<input type="text" id="author" name="author" class="form-control" value='<%=rs.getString("b_author") %>'>
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 분류</label>
						<div class="col-sm-3">
							<input type="text" id="category" name="category" class="form-control" value='<%=rs.getString("b_category") %>'>
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 재고 수</label>
						<div class="col-sm-3">
							<input type="text" id="unitInStock" name="unitInStock" class="form-control" value='<%=rs.getString("b_unitInStock") %>'>
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 총 페이지 수</label>
						<div class="col-sm-3">
							<input type="text" id="totalPages" name="totalPages" class="form-control" value='<%=rs.getString("b_totalPages") %>'>
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 출판일</label>
						<div class="col-sm-3">
							<input type="text" id="ReleaseDate" name="ReleaseDate" class="form-control" value='<%=rs.getString("b_releaseDate") %>'>
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 상태</label>
						<div class="col-sm-5">
							<input type="radio" name="condition" value="New"> 신규 제품
							<input type="radio" name="condition" value="Old"> 중고 제품
							<input type="radio" name="condition" value="Refurbished"> 재생 제품
						</div>
					</div>
					<div class="form-group row">
						<label class="col-sm-2"> 이미지</label>
						<div class="col-sm-5">
							<input type="file" name="bookImage" class="form-control">
						</div>
					</div>
					<div class="form-group row">
						<div class="col-sm-offset-2 col-sm-10">
							<input type="submit" class ="btn btn-primary" value="등록">
						</div>
					</div>
				</form>
			</div>
		</div>
	</div>
	<%
		}
		if (rs != null)
			rs.close();
		if (pstmt != null)
			pstmt.close();
		if (conn != null)
			conn.close();
	%>
</body>
</html>