import java.io.File;
import java.io.FileInputStream;
import java.io.InputStream;

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
	public static int uerCnt; 
	
	public RhythemDetection(){
		uerCnt = 0;
	}
		
	public static void UER(int e){
		System.out.println("Your speech has had " + e + "gaps in it");		
		
		//start of UER
		if(e>6){ //need to have a timer for the course of the speech
			System.out.println("You have had several gaps in your speech \n perhaps you should try to [suggestion]" );
		}
	}
	
                         
    public static void main(String[] args) throws Exception {
    	
    	
        //used to supply required and optional attributes to recognizer                  
        
    	System.out.println("Loading...");
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
     while ((result = recognizer.getResult()) != null) {
         System.out.format("#### New Hypothesis: %s\n", result.getHypothesis());
         
      // Get individual words and their times.
         for (WordResult r : result.getWords()) {
             System.out.println("t stamp: " + r);
             s = r;
             if((s.getTimeFrame().getEnd()-s.getTimeFrame().getEnd())>1600){ //TODO have a global average too //if the time bewteen two words is greater than 1600
             	uerCnt++;
             }
         }
         UER(uerCnt); 
     }
     recognizer.stopRecognition();
     
    
     /*
        //used this portion for testing: as this is a file it allows for consistency with results
        StreamSpeechRecognizer recognizer = new StreamSpeechRecognizer(
                configuration);
        InputStream stream = new FileInputStream(new File("rec_10s.wav"));

        recognizer.startRecognition(stream);
        SpeechResult result = null;
        WordResult s = null; 
         
        
        while ((result = recognizer.getResult()) != null) {
            System.out.format("Hypothesis: %s\n", result.getHypothesis());
          
            for (WordResult r : result.getWords()) {
                System.out.println("t stamp: " + r.getTimeFrame().getEnd()); //gets that actual time stamps of "words"
                s = r; 
              //UER
                if((s.getTimeFrame().getEnd()-s.getTimeFrame().getEnd())>1600){ //TODO have a global average too 
                	uerCnt++; 
                }
            }    
        }
        recognizer.stopRecognition();     
        
        */ 
    }
}