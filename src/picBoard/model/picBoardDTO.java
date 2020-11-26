package picBoard.model;

public class picBoardDTO {
	
	private int num;					//글 번호
	private String id;					//글 작성한 유저의 아이디
	private String name;				//이름, 글쓴이 
	private String subject;				//제목
	private String address;				//찍은 장소
	private String description;			//설명
	private String camera;				//사용한 카메라
	private String filter;				//사용한 필터 or 어플리케이션
	private int hit;					//조회수
	private String photoTime;			//사진 촬영한 시간
	private String category;			//분류
	private String ip;					//아이피 
	private String regist_day;			//등록 시간
	private String filename;			//파일 이름
	private long filesize;				//파일 크기
	private int ripple_count;
	
	public picBoardDTO() {
		super();		
	}

	public int getNum() {
		return num;
	}

	public void setNum(int num) {
		this.num = num;
	}

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getRegist_day() {
		return regist_day;
	}

	public void setRegist_day(String regist_day) {
		this.regist_day = regist_day;
	}

	public int getHit() {
		return hit;
	}

	public void setHit(int hit) {
		this.hit = hit;
	}

	public String getIp() {
		return ip;
	}

	public void setIp(String ip) {
		this.ip = ip;
	}

	public String getAddress() {
		return address;
	}

	public void setAddress(String address) {
		this.address = address;
	}

	public String getDescription() {
		return description;
	}

	public void setDescription(String description) {
		this.description = description;
	}

	public String getCamera() {
		return camera;
	}

	public void setCamera(String camera) {
		this.camera = camera;
	}

	public int getRipple_count() {
		return ripple_count;
	}

	public void setRipple_count(int ripple_count) {
		this.ripple_count = ripple_count;
	}

	public long getFilesize() {
		return filesize;
	}

	public void setFilesize(long filesize) {
		this.filesize = filesize;
	}

	public String getFilename() {
		return filename;
	}

	public void setFilename(String filename) {
		this.filename = filename;
	}

	public String getCategory() {
		return category;
	}

	public void setCategory(String category) {
		this.category = category;
	}

	public String getPhotoTime() {
		return photoTime;
	}

	public void setPhotoTime(String photoTime) {
		this.photoTime = photoTime;
	}

	public String getFilter() {
		return filter;
	}

	public void setFilter(String filter) {
		this.filter = filter;
	}

	public String getSubject() {
		return subject;
	}

	public void setSubject(String subject) {
		this.subject = subject;
	}

}
