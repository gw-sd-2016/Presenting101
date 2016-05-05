package rhythm;


import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.InputStream;
import java.io.PrintWriter;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;

import edu.cmu.sphinx.api.Configuration;
import edu.cmu.sphinx.api.LiveSpeechRecognizer;
import edu.cmu.sphinx.api.SpeechResult;
import edu.cmu.sphinx.api.StreamSpeechRecognizer;
import edu.cmu.sphinx.result.WordResult;

/**
 * @author Terrence Lewis 
 * Using CMU's Sphinx library for transcribing
 * Class is designed to be the beginning of the
 * rhythm detection - this is the basics and 
 * attempts to find words per minute  
 * 
 * http://cmusphinx.sourceforge.net/wiki/tutorialsphinx4
 */
public class RhythemDetection {
	public static int uerCnt, fast, medium, slow; 
	String speed; 
	public RhythemDetection() throws Exception{
		uerCnt = 0;
		fast =0; 
		medium =0; 
		slow = 0; 
		speed = "";
		 
		Rdet(); 
	}
		
	public void Rdet() throws Exception{
		System.out.println("Loading 649...");
    	Configuration configuration = new Configuration();

        configuration
                .setAcousticModelPath("resources/edu/cmu/sphinx/models/en-us/en-us");
        configuration
                .setDictionaryPath("resources/edu/cmu/sphinx/models/en-us/cmudict-en-us.dict");
        configuration
                .setLanguageModelPath("resources/edu/cmu/sphinx/models/en-us/en-us.lm.bin");
     
        
    
     //gets live speech data 
     //should note that it is not accurate using it just for word detection and time signatures  
     LiveSpeechRecognizer recognizer = new LiveSpeechRecognizer(configuration);
     // Start recognition process pruning previously cached data.
     recognizer.startRecognition(true);
     SpeechResult result = recognizer.getResult();
     // Pause recognition process. It can be resumed then with startRecognition(false).
    // recognizer.stopRecognition();
     System.out.println("Ready!");
     WordResult s = null; 
     ArrayList<Integer> tStamp = new ArrayList<Integer>();
     int lastEntry = 0; //this is to be used for detecting gaps between two sentences i.e the end of one & start of another
     boolean started = false; 
     while ((result = recognizer.getResult()) != null) {
         System.out.format("#### New Hypothesis: %s\n", result.getHypothesis());
         
      // Get individual words and their times.
         for (WordResult r : result.getWords()) {
             System.out.println("t stamp: " + r + r.getTimeFrame().getEnd());
             
             s = r;
             int t = (int) r.getTimeFrame().getEnd(); 
             tStamp.add(t); 
             System.out.println("#### " + (r.getTimeFrame().getEnd()-s.getTimeFrame().getEnd()));
            if(started){
            	if((tStamp.get(0)-lastEntry)>4000){ //this checks the end of one sentence and start of another
            		System.out.println("you are taking to many pauses: "+ (tStamp.get(0)-lastEntry));
            		uerCnt++; 
            	}
            }
        
             if(tStamp.size()>2){ //need a case for just one word said
            	 for(int i=0; i<tStamp.size()-1; i++){
            	
            		 if((tStamp.get(i+1))-(tStamp.get(i))<100){
            			 System.out.println("greater than 300"); //talking too fast
            			 fast++; 
            		 }
            		 else if((tStamp.get(i+1))-(tStamp.get(i))>110 && (tStamp.get(i+1))-(tStamp.get(i))<899){
            			 System.out.println("You are talking right speed"); //talking right speed
            			 medium++; 
            		 }
            		 else if((tStamp.get(i+1))-(tStamp.get(i))>900){
            			 System.out.println("you are talking to slow"); //talking to slowly
            			 slow++; 
            		 }
            	 }
            }
            // if((s.getTimeFrame().getEnd()-s.getTimeFrame().getEnd())>1600){ //TODO have a global average too //if the time bewteen two words is greater than 1600
             //uerCnt++;
             //}
         }
         
         System.out.println("First entry " + tStamp.get(0) + " last entry " + tStamp.get(tStamp.size()-1)); 
         lastEntry = tStamp.get(tStamp.size()-1);
         started = true; 
         tStamp.clear();
         
         if(fast>medium && fast>slow){
        	 speed = "fast";
         }
         else if(medium>fast && medium>slow){
        	 speed = "medium"; 
         }
         else if(slow>fast && slow>medium){
        	 speed = "slow"; 
         }
         UER(uerCnt, speed); 
     }
     recognizer.stopRecognition();
	
	}
	
	
	public static void UER(int e, String flow){ //flow = good or bad 
		System.out.println("Your speech has had " + e + "gaps in it");		
		
		//start of UER
		if(e>6){ //need to have a timer for the course of the speech
			System.out.println("You have had several gaps in your speech \n perhaps you should try to [suggestion]" );
		}
		
		try {
			PrintWriter writer = new PrintWriter("C:/Users/tlewis/Desktop/rhythm.txt", "UTF-8");
			writer.println(e);
			writer.println(flow);
			writer.flush();
			writer.close();
		} catch (FileNotFoundException | UnsupportedEncodingException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
	}
	
                         
    public static void main(String[] args) throws Exception {
    	new RhythemDetection(); 
    	
        //used to supply required and optional attributes to recognizer                  
    }
}