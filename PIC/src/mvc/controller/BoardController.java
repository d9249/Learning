package mvc.controller;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Enumeration;
import java.util.List;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.oreilly.servlet.MultipartRequest;
import com.oreilly.servlet.multipart.DefaultFileRenamePolicy;

import mvc.model.BoardDAO;
import mvc.model.BoardDTO;

public class BoardController extends HttpServlet {
	private static final long serialVersionUID = 1L;
	static final int LISTCOUNT = 4;

	public void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		doPost(request, response);
	}

	public void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		
		String RequestURI = request.getRequestURI();
		String contextPath = request.getContextPath();
		String command = RequestURI.substring(contextPath.length());
		
		response.setContentType("text/html; charset=utf-8");
		request.setCharacterEncoding("utf-8");
	
		if (command.equals("/BoardListAction.do")) {//��ϵ� �� ��� ������ ����ϱ�
			requestBoardList(request);
			RequestDispatcher rd = request.getRequestDispatcher("./board/list.jsp");
			rd.forward(request, response);
		} else if (command.equals("/BoardWriteForm.do")) { // �� ��� ������ ����ϱ�
				requestLoginName(request);
				RequestDispatcher rd = request.getRequestDispatcher("./board/writeForm.jsp");
				rd.forward(request, response);				
		} else if (command.equals("/BoardWriteAction.do")) {// ���ο� �� ����ϱ�
				requestBoardWrite(request);
				RequestDispatcher rd = request.getRequestDispatcher("/BoardListAction.do");
				rd.forward(request, response);						
		} else if (command.equals("/BoardViewAction.do")) {//���õ� �� �� ������ ��������
				requestBoardView(request);
				RequestDispatcher rd = request.getRequestDispatcher("/BoardView.do");
				rd.forward(request, response);						
		} else if (command.equals("/BoardView.do")) { //�� �� ������ ����ϱ�
				RequestDispatcher rd = request.getRequestDispatcher("./board/view.jsp");
				rd.forward(request, response);	
		} else if (command.equals("/BoardUpdateAction.do")) { //���õ� ���� ��ȸ�� �����ϱ�
				requestBoardUpdate(request);
				RequestDispatcher rd = request.getRequestDispatcher("/BoardListAction.do");
				rd.forward(request, response);
		}else if (command.equals("/BoardDeleteAction.do")) { //���õ� �� �����ϱ�
				requestBoardDelete(request);
				RequestDispatcher rd = request.getRequestDispatcher("/BoardListAction.do");
				rd.forward(request, response);				
		} 
	}
	//��ϵ� �� ��� ��������	
	public void requestBoardList(HttpServletRequest request){
			
		BoardDAO dao = BoardDAO.getInstance();
		List<BoardDTO> boardlist = new ArrayList<BoardDTO>();
		
	  	int pageNum=1;
		int limit=LISTCOUNT;
		
		if(request.getParameter("pageNum")!=null)
			pageNum=Integer.parseInt(request.getParameter("pageNum"));
				
		String items = request.getParameter("items");
		String text = request.getParameter("text");
		
		int total_record=dao.getListCount(items, text);
		boardlist = dao.getBoardList(pageNum,limit, items, text); 
		
		int total_page;
		
		if (total_record % limit == 0){     
	     	total_page =total_record/limit;
	     	Math.floor(total_page);  
		}
		else{
		   total_page =total_record/limit;
		   Math.floor(total_page); 
		   total_page =  total_page + 1; 
		}		
   
   		request.setAttribute("pageNum", pageNum);		  
   		request.setAttribute("total_page", total_page);   
		request.setAttribute("total_record",total_record); 
		request.setAttribute("boardlist", boardlist);								
	}
	//������ ����ڸ� ��������
	public void requestLoginName(HttpServletRequest request){
					
		String id = request.getParameter("id");
		
		BoardDAO  dao = BoardDAO.getInstance();
		
		String name = dao.getLoginNameById(id);		
		
		request.setAttribute("name", name);									
	}
	// ���ο� �� ����ϱ�
	public void requestBoardWrite(HttpServletRequest request) throws IOException{
					
		BoardDAO dao = BoardDAO.getInstance();
		
		BoardDTO board = new BoardDTO();
		
		String path = request.getSession().getServletContext().getRealPath("/resources/images");

	    int size = 1024 * 1024 * 10; // ���尡���� ���� ũ��

	    MultipartRequest multi = new MultipartRequest(request, path, size, "UTF-8", new DefaultFileRenamePolicy());
	    Enumeration files = multi.getFileNames();
	    
		board.setId(multi.getParameter("id"));
		board.setName(multi.getParameter("name"));
		board.setSubject(multi.getParameter("subject"));
		board.setAddress(multi.getParameter("address"));
		board.setDescription(multi.getParameter("description"));
		board.setCamera(multi.getParameter("camera"));
		board.setFilter(multi.getParameter("filter"));
		board.setPhotoTime(multi.getParameter("photoTime"));
		board.setCategory(multi.getParameter("category"));
		board.setFilename(multi.getFilesystemName("filename"));
		
		System.out.println(request.getParameter("name"));
		System.out.println(request.getParameter("subject"));
		System.out.println(request.getParameter("address"));
		System.out.println(request.getParameter("description"));
		System.out.println(request.getParameter("camera"));
		System.out.println(request.getParameter("filter"));
		System.out.println(request.getParameter("photoTime"));
		System.out.println(request.getParameter("category"));
		System.out.println(request.getParameter("filename"));		
		java.text.SimpleDateFormat formatter = new java.text.SimpleDateFormat("yyyy/MM/dd(HH:mm:ss)");
		String regist_day = formatter.format(new java.util.Date());
		
		board.setHit(0);
		board.setRegist_day(regist_day);
		board.setIp(request.getRemoteAddr());			
		
		dao.insertBoard(board);					
	}
	//���õ� �� �� ������ ��������
	public void requestBoardView(HttpServletRequest request){
					
		BoardDAO dao = BoardDAO.getInstance();
		int num = Integer.parseInt(request.getParameter("num"));
		int pageNum = Integer.parseInt(request.getParameter("pageNum"));	
		
		BoardDTO board = new BoardDTO();
		board = dao.getBoardByNum(num, pageNum);		
		
		request.setAttribute("num", num);		 
   		request.setAttribute("page", pageNum); 
   		request.setAttribute("board", board);   									
	}
	//���õ� �� ���� �����ϱ�
	public void requestBoardUpdate(HttpServletRequest request) throws IOException{
					
		int num = Integer.parseInt(request.getParameter("num"));
		int pageNum = Integer.parseInt(request.getParameter("pageNum"));	
		
		String path = request.getSession().getServletContext().getRealPath("/resources/images");

	    int size = 1024 * 1024 * 10; // ���尡���� ���� ũ��

	    MultipartRequest multi = new MultipartRequest(request, path, size, "UTF-8", new DefaultFileRenamePolicy());
	    Enumeration files = multi.getFileNames();
		
		BoardDAO dao = BoardDAO.getInstance();		
		
		BoardDTO board = new BoardDTO();		
		board.setNum(num);
		board.setName(multi.getParameter("name"));
		board.setSubject(multi.getParameter("subject"));
		board.setAddress(multi.getParameter("address"));
		board.setDescription(multi.getParameter("description"));
		board.setCamera(multi.getParameter("camera"));
		board.setFilter(multi.getParameter("filter"));
		board.setPhotoTime(multi.getParameter("photoTime"));
		board.setCategory(multi.getParameter("category"));
		board.setFilename(multi.getFilesystemName("filename"));
		
		 java.text.SimpleDateFormat formatter = new java.text.SimpleDateFormat("yyyy/MM/dd(HH:mm:ss)");
		 String regist_day = formatter.format(new java.util.Date()); 
		 
		 board.setHit(0);
		 board.setRegist_day(regist_day);
		 board.setIp(request.getRemoteAddr());
		
		 dao.updateBoard(board);								
	}
	//���õ� �� �����ϱ�
	public void requestBoardDelete(HttpServletRequest request){
					
		int num = Integer.parseInt(request.getParameter("num"));
		int pageNum = Integer.parseInt(request.getParameter("pageNum"));	
		
		BoardDAO dao = BoardDAO.getInstance();
		dao.deleteBoard(num);							
	}	
}
