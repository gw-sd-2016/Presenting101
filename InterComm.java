package ipct;

import java.io.RandomAccessFile;
//code found on http://v01ver-howto.blogspot.co.uk/2010/04/howto-use-named-pipes-to-communicate.html

public class InterComm {
	public static void main(String[] args){
		
		try {
			// Connect to the pipe
			RandomAccessFile pipe = new RandomAccessFile("\\\\.\\pipe\\testpipe", "rw");
			String echoText = "test 2\n";
			// write to pipe
			pipe.write ( echoText.getBytes() );
			// read response
			String echoResponse = pipe.readLine();
			System.out.println("Response: " + echoResponse );
			pipe.close();

			} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			}
	}

}
