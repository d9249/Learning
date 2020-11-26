package picBoard.model;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;

import mvc.database.DBConnection;

public class picBoardDAO {

	private static picBoardDAO instance;
	
	private picBoardDAO() {
		
	}

	public static picBoardDAO getInstance() {
		if (instance == null)
			instance = new picBoardDAO();
		return instance;
	}	
	//board 테이블의 레코드 개수
	public int getListCount(String items, String text) {
		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;

		int x = 0;

		String sql;
		
		if (items == null && text == null)
			sql = "select count(*) from board";
		else
			sql = "SELECT count(*) FROM board where " + items + " like '%" + text + "%'";
		
		try {
			conn = DBConnection.getConnection();
			pstmt = conn.prepareStatement(sql);
			rs = pstmt.executeQuery();

			if (rs.next()) 
				x = rs.getInt(1);
			
		} catch (Exception ex) {
			System.out.println("getListCount() error : " + ex);
		} finally {			
			try {				
				if (rs != null) 
					rs.close();							
				if (pstmt != null) 
					pstmt.close();				
				if (conn != null) 
					conn.close();												
			} catch (Exception ex) {
				throw new RuntimeException(ex.getMessage());
			}		
		}		
		return x;
	}
	//board 테이블의 레코드 가져오기
	public ArrayList<picBoardDTO> getBoardList(int page, int limit, String items, String text) {
		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;

		int total_record = getListCount(items, text );
		int start = (page - 1) * limit;
		int index = start + 1;

		String sql;

		if (items == null && text == null)
			sql = "select * from board ORDER BY num DESC";
		else
			sql = "SELECT  * FROM board where " + items + " like '%" + text + "%' ORDER BY num DESC ";

		ArrayList<picBoardDTO> list = new ArrayList<picBoardDTO>();

		try {
			conn = DBConnection.getConnection();
			pstmt = conn.prepareStatement(sql);
			rs = pstmt.executeQuery();

			while (rs.absolute(index)) {
				picBoardDTO picboard = new picBoardDTO();
				picboard.setNum(rs.getInt("num"));
				picboard.setId(rs.getString("id"));
				picboard.setName(rs.getString("name"));
				picboard.setSubject(rs.getString("subject"));
				picboard.setAddress(rs.getString("address"));
				picboard.setDescription(rs.getString("description"));
				picboard.setCamera(rs.getString("camera"));
				picboard.setFilter(rs.getString("filter"));
				picboard.setHit(rs.getInt("hit"));
				picboard.setPhotoTime(rs.getString("photoTime"));
				picboard.setCategory(rs.getString("category"));
				picboard.setIp(rs.getString("ip"));
				picboard.setRegist_day(rs.getString("regist_day"));
				picboard.setFilename(rs.getString("filename"));
				picboard.setFilesize(rs.getLong("filesize"));
				picboard.setRipple_count(rs.getInt("ripple_count"));
				
				list.add(picboard);

				if (index < (start + limit) && index <= total_record)
					index++;
				else
					break;
			}
			return list;
		} catch (Exception ex) {
			System.out.println("getBoardList() error : " + ex);
		} finally {
			try {
				if (rs != null) 
					rs.close();							
				if (pstmt != null) 
					pstmt.close();				
				if (conn != null) 
					conn.close();
			} catch (Exception ex) {
				throw new RuntimeException(ex.getMessage());
			}			
		}
		return null;
	}
	//member 테이블에서 인증된 id의 사용자명 가져오기
	public String getLoginNameById(String id) {
		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;	

		String name=null;
		String sql = "select * from member where id = ? ";

		try {
			conn = DBConnection.getConnection();
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, id);
			rs = pstmt.executeQuery();

			if (rs.next()) 
				name = rs.getString("name");	
			
			return name;
		} catch (Exception ex) {
			System.out.println("getBoardByNum() error : " + ex);
		} finally {
			try {				
				if (rs != null) 
					rs.close();							
				if (pstmt != null) 
					pstmt.close();				
				if (conn != null) 
					conn.close();
			} catch (Exception ex) {
				throw new RuntimeException(ex.getMessage());
			}		
		}
		return null;
	}

	//board 테이블에 새로운 글 삽입하기
	public void insertBoard(picBoardDTO board)  {

		
		
		Connection conn = null;
		PreparedStatement pstmt = null;
		try {
			conn = DBConnection.getConnection();		

			String sql = "insert into picBoard values(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
		
			pstmt = conn.prepareStatement(sql);
			pstmt.setInt(1, board.getNum());
			pstmt.setString(2, board.getId());
			pstmt.setString(3, board.getName());
			pstmt.setString(4, board.getSubject());
			pstmt.setString(5, board.getAddress());
			pstmt.setString(6, board.getDescription());
			pstmt.setString(7, board.getCamera());
			pstmt.setString(8, board.getFilter());
			pstmt.setInt(9, board.getHit());
			pstmt.setString(10, board.getPhotoTime());
			pstmt.setString(11, board.getCategory());
			pstmt.setString(12, board.getIp());
			pstmt.setString(13, board.getRegist_day());
			pstmt.setString(14, board.getFilename());
			pstmt.setLong(15, board.getFilesize());
			pstmt.setInt(16, board.getRipple_count());

			pstmt.executeUpdate();
		} catch (Exception ex) {
			System.out.println("insertBoard() error : " + ex);
		} finally {
			try {									
				if (pstmt != null) 
					pstmt.close();				
				if (conn != null) 
					conn.close();
			} catch (Exception ex) {
				throw new RuntimeException(ex.getMessage());
			}		
		}		
	} 
	//선택된 글의 조회수 증가하기
	public void updateHit(int num) {

		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		
		try {
			conn = DBConnection.getConnection();

			String sql = "select hit from picBoard where num = ? ";
			pstmt = conn.prepareStatement(sql);
			pstmt.setInt(1, num);
			rs = pstmt.executeQuery();
			int hit = 0;

			if (rs.next())
				hit = rs.getInt("hit") + 1;
		

			sql = "update picBoard set hit=? where num=?";
			pstmt = conn.prepareStatement(sql);		
			pstmt.setInt(1, hit);
			pstmt.setInt(2, num);
			pstmt.executeUpdate();
		} catch (Exception ex) {
			System.out.println("updateHit() error : " + ex);
		} finally {
			try {
				if (rs != null) 
					rs.close();							
				if (pstmt != null) 
					pstmt.close();				
				if (conn != null) 
					conn.close();
			} catch (Exception ex) {
				throw new RuntimeException(ex.getMessage());
			}			
		}
	}
	//선택된 글 상세 내용 가져오기
	public picBoardDTO getBoardByNum(int num, int page) {
		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		picBoardDTO picboard = null;

		updateHit(num);
		String sql = "select * from picBoard where num = ? ";

		try {
			conn = DBConnection.getConnection();
			pstmt = conn.prepareStatement(sql);
			pstmt.setInt(1, num);
			rs = pstmt.executeQuery();

			if (rs.next()) {
				picboard = new picBoardDTO();
				picboard.setNum(rs.getInt("num"));
				picboard.setId(rs.getString("id"));
				picboard.setName(rs.getString("name"));
				picboard.setSubject(rs.getString("subject"));
				picboard.setAddress(rs.getString("address"));
				picboard.setDescription(rs.getString("description"));
				picboard.setCamera(rs.getString("camera"));
				picboard.setFilter(rs.getString("filter"));
				picboard.setHit(rs.getInt("hit"));
				picboard.setPhotoTime(rs.getString("photoTime"));
				picboard.setCategory(rs.getString("category"));
				picboard.setIp(rs.getString("ip"));
				picboard.setRegist_day(rs.getString("regist_day"));
				picboard.setFilename(rs.getString("filename"));
				picboard.setFilesize(rs.getLong("filesize"));
				picboard.setRipple_count(rs.getInt("ripple_count"));
			}
			
			return picboard;
		} catch (Exception ex) {
			System.out.println("getBoardByNum() error : " + ex);
		} finally {
			try {
				if (rs != null) 
					rs.close();							
				if (pstmt != null) 
					pstmt.close();				
				if (conn != null) 
					conn.close();
			} catch (Exception ex) {
				throw new RuntimeException(ex.getMessage());
			}		
		}
		return null;
	}
	//선택된 글 내용 수정하기
	public void updateBoard(picBoardDTO board) {

		Connection conn = null;
		PreparedStatement pstmt = null;
	
		try {
			String sql = "update picBoard set name=?, subject=?, address=?, description=?, "
					+ "camera=?, filter=?, photoTime=?, category=?, filename=?, filesize=? where num=?";

			conn = DBConnection.getConnection();
			pstmt = conn.prepareStatement(sql);
			
			conn.setAutoCommit(false);

			pstmt.setString(1, board.getName());
			pstmt.setString(2, board.getSubject());
			pstmt.setString(3, board.getAddress());
			pstmt.setString(4, board.getDescription());
			pstmt.setString(5, board.getCamera());
			pstmt.setString(6, board.getFilter());
			pstmt.setString(7, board.getPhotoTime());
			pstmt.setString(8, board.getCategory());
			pstmt.setString(9, board.getFilename());
			pstmt.setLong(10, board.getFilesize());
			pstmt.setInt(11, board.getNum());
			
			pstmt.executeUpdate();			
			conn.commit();

		} catch (Exception ex) {
			System.out.println("updateBoard() error : " + ex);
		} finally {
			try {										
				if (pstmt != null) 
					pstmt.close();				
				if (conn != null) 
					conn.close();
			} catch (Exception ex) {
				throw new RuntimeException(ex.getMessage());
			}		
		}
	} 
	//선택된 글 삭제하기
	public void deleteBoard(int num) {
		Connection conn = null;
		PreparedStatement pstmt = null;		

		String sql = "delete from board where num=?";	

		try {
			conn = DBConnection.getConnection();
			pstmt = conn.prepareStatement(sql);
			pstmt.setInt(1, num);
			pstmt.executeUpdate();

		} catch (Exception ex) {
			System.out.println("deleteBoard() error : " + ex);
		} finally {
			try {										
				if (pstmt != null) 
					pstmt.close();				
				if (conn != null) 
					conn.close();
			} catch (Exception ex) {
				throw new RuntimeException(ex.getMessage());
			}		
		}
	}	
}
