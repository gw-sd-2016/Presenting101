/*
 * Copyright 1999-2004 Carnegie Mellon University.
 * Portions Copyright 2004 Sun Microsystems, Inc.
 * Portions Copyright 2004 Mitsubishi Electric Research Laboratories.
 * All Rights Reserved.  Use is subject to license terms.
 *
 * See the file "license.terms" for information on usage and
 * redistribution of this file, and for a DISCLAIMER OF ALL
 * WARRANTIES.
 *
 */

package demo.sphinx.helloworld;
import java.io.File;
import java.io.FileInputStream;
import java.io.InputStream;

import edu.cmu.sphinx.api.Configuration;
import edu.cmu.sphinx.api.LiveSpeechRecognizer;
import edu.cmu.sphinx.api.SpeechResult;
import edu.cmu.sphinx.api.StreamSpeechRecognizer;

import edu.cmu.sphinx.result.WordResult;


import edu.cmu.sphinx.api.Configuration;
import edu.cmu.sphinx.api.LiveSpeechRecognizer;
import edu.cmu.sphinx.api.SpeechResult;

import edu.cmu.sphinx.frontend.util.Microphone;
import edu.cmu.sphinx.recognizer.Recognizer;
import edu.cmu.sphinx.result.Result;
import edu.cmu.sphinx.result.WordResult;
import edu.cmu.sphinx.util.props.ConfigurationManager;
import edu.cmu.sphinx.util.props.PropertyException;

import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.io.File;
import java.io.IOException;
import java.net.URL;

/**
 * A simple demo showing a simple speech application recognition
 * built using Sphinx-4. This application uses the Sphinx-4 endpointer,
 * which automatically segments incoming audio into utterances and silences.
 */


/*I have modified portions of this open source code (credits above) so that is may fit my project. 
 * What is here still needs to be modified- as it does the bare minimum for 
 * what needs to be done in my project. Much of what I modified for this to work is in the .gram file
 * which it uses know what words to look for. I will be using this class to detect specific "bad words" 
 * of users.   
 * Terrence Lewis
 * */
public class HelloWorld2 {
	public static int uerCnt; 
	
	public HelloWorld2(String[] args) throws Exception{
		int umm, ahh, basically;  
		uerCnt = 0;
		
		badWords(args);
		//rhythemDet(); 
	} 

	
	//beginnings of the user error report for bad word detection
	public static void UER(String e, String f, int g){
		
		System.out.println("You've said a total of " + e + " bad words" );	
		
	}
	
	
	public void badWords(String[] args){
		try {
            URL url;
            if (args.length > 0) {
                url = new File(args[0]).toURI().toURL();
            } else {
                url = HelloWorld2.class.getResource("helloworld.config.xml");
            }

            System.out.println("Loading v:2.2.0...");

            ConfigurationManager cm = new ConfigurationManager(url);

	    Recognizer recognizer = (Recognizer) cm.lookup("recognizer");
	    Microphone microphone = (Microphone) cm.lookup("microphone");


            // allocate the resource necessary for the recognizer 
            recognizer.allocate();

            // the microphone will keep recording until the program exits 
	    if (microphone.startRecording()) {
	    	int count =0; 
	    //to look for these words the .gram file must be modified
		System.out.println
		    ("Current Bad Words: (Good morning | basically | umm | test | ahh)");
		
		while (true) {
		    System.out.println
			("Start speaking. \n");
		    
                    //
                     //* This method will return when the end of speech
                     //* is reached. Note that the endpointer will determine
                     //* the end of speech.
                     
		    Result result = recognizer.recognize();
		    
		    if (result != null) {
			String resultText = result.getBestFinalResultNoFiller();
			//String reT = result.getTimedBestResult(true, true); //testing words with time stamps
			System.out.println("You said: " + resultText + "\n");
			//System.out.println(result.getFrameStatistics());
			//new MKeyListener(); 
			
		//	System.out.println("== "+ reT);
			count++; //counts bad words -> will add functionality to count individual bad words
			//UER(Integer.toString(count), resultText, (Integer) null); //sends this to be printed
		  
			new KeyListenerExample(); 
		    } else {
			System.out.println("I can't hear what you said.\n");
		    }
		}
	    } else {
		System.out.println("Cannot start microphone.");
		recognizer.deallocate();
		System.exit(1);
	    }
        } catch (IOException e) {
            System.err.println("Problem when loading HelloWorld: " + e);
            e.printStackTrace();
        } catch (PropertyException e) {
            System.err.println("Problem configuring HelloWorld: " + e);
            e.printStackTrace();
        }
	}
	 
	
	
	public void rhythemDet() throws Exception{
		System.out.println("Loading Config v:2.2.0 \n");
		
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
             	//((s.getTimeFrame().getEnd()-s.getTimeFrame().getEnd())>1600)
             	//((s.getStartFrame()-s.getEndFrame())>1600)
             }
         }
        // UER(uerCnt); 
     }
     recognizer.stopRecognition();
	}
	
	
    /**
     * Main method for running the HelloWorld demo.
     * @throws Exception 
     */
    public static void main(String[] args) throws Exception {
       new HelloWorld2(args);
    	
    	
    	
        
    }
}



class KeyListenerExample {
   KeyListener listener = new KeyListener() {

@Override

public void keyPressed(KeyEvent event) {

}

@Override
public void keyReleased(KeyEvent arg0) {
	// TODO Auto-generated method stub
	
}

@Override
public void keyTyped(KeyEvent e) {
	// TODO Auto-generated method stub
	
}
   };
   
   }
  

