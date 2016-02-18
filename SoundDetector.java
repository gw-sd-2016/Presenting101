/*
* -------------------------------------------------------------
*
* TarsosDSP is developed by Joren Six at IPEM, University Ghent
*  
* -------------------------------------------------------------
*
*  Info: http://0110.be/tag/TarsosDSP
*  Github: https://github.com/JorenSix/TarsosDSP
*  Releases: http://0110.be/releases/TarsosDSP/
*  
*  TarsosDSP includes modified source code by various authors,
*  for credits and info, see README.
* 
*/


package retest;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.lang.reflect.InvocationTargetException;
import java.util.ArrayList;
import java.util.List;

import javax.sound.sampled.AudioFormat;
import javax.sound.sampled.AudioInputStream;
import javax.sound.sampled.AudioSystem;
import javax.sound.sampled.Clip;
import javax.sound.sampled.DataLine;
import javax.sound.sampled.LineUnavailableException;
import javax.sound.sampled.Mixer;
import javax.sound.sampled.Mixer.Info;
import javax.sound.sampled.TargetDataLine;
import javax.sound.sampled.UnsupportedAudioFileException;
import javax.swing.ButtonGroup;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JRadioButton;
import javax.swing.JScrollPane;
import javax.swing.JSlider;
import javax.swing.JTextArea;
import javax.swing.SwingUtilities;
import javax.swing.border.TitledBorder;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;

import be.tarsos.dsp.AudioDispatcher;
import be.tarsos.dsp.AudioEvent;
import be.tarsos.dsp.AudioProcessor;
import be.tarsos.dsp.SilenceDetector;
import be.tarsos.dsp.example.InputPanel;
import be.tarsos.dsp.example.Shared;
import be.tarsos.dsp.io.jvm.JVMAudioInputStream;



/*I have modified portions of this open source code (credits above) so that is may fit my project. 
 * What is here still needs to be modified- as it does the bare minimum for 
 * what needs to be done in my project. I will be using this class for detecting volume of
 * a speaker. 
 * Terrence Lewis
 * */

public class SoundDetector extends JFrame implements AudioProcessor {

	private static final long serialVersionUID = 3501426880288136245L;

	private final JTextArea textArea;
	ArrayList<Clip> clipList;
	int counter;
	double threshold,tbd;
	AudioDispatcher dispatcher;
	Mixer currentMixer;
	private final GaphPanel graphPanel;
	SilenceDetector silenceDetector;
	private int avg, cnt, pnt;
	Info test = Shared.getMixerInfo(false,true).get(0); 
	Mixer nv =  AudioSystem.getMixer(test);
	Mixer mix = null; 
	JPanel inputPanel; 


	
	public SoundDetector() throws LineUnavailableException {
		this.setLayout(new BorderLayout());
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setTitle("Sound Detector");
		this.threshold = SilenceDetector.DEFAULT_SILENCE_THRESHOLD;
		/*
		JPanel inputPanel = new InputPanel();
		this.setLayout(new BorderLayout());
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setTitle("Volume Detection");
		this.threshold = SilenceDetector.DEFAULT_SILENCE_THRESHOLD;
		this.tbd = SilenceDetector.DEFAULT_SILENCE_THRESHOLD;
		cnt =0; 
		avg =0;
		*/ 
		
		//JPanel buttonPanel = new JPanel(new GridLayout(0,1));
		//ButtonGroup group = new ButtonGroup();
		
		
		inputPanel = new InputPanel();
		//add(inputPanel);
		inputPanel.setBorder(new TitledBorder ("test"));
		
		/*
		inputPanel = new JPanel(new GridLayout(0,1));
		JRadioButton button = new JRadioButton();
		button.setText(Shared.toLocalString(test));
		buttonPanel.add(button);
		group.add(button);
		button.setActionCommand(test.toString());
		inputPanel.add(new JScrollPane(buttonPanel,JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED,JScrollPane.HORIZONTAL_SCROLLBAR_NEVER),BorderLayout.CENTER);
		
		button.addActionListener(setInput);
		*/
		
		//button.setSelected(true);
		Shared.getMixerInfo(false, true); 
		
		
		
		
			System.out.println("&& " + //info.toString());
			Shared.getMixerInfo(false,true).get(0)); 
		
		//this.firePropertyChange("mixer", mixer, newValue);
			
		
			/*
			inputPanel.addPropertyChangeListener("nv", 
					new PropertyChangeListener() {
						@Override
						public void propertyChange(PropertyChangeEvent arg0) {
							try {
								
								setNewMixer(nv);
								//System.out.println("###!!"  + arg0);
								//com.sun.media.sound.DirectAudioDevice@74b21e6a
							} catch (LineUnavailableException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							} catch (UnsupportedAudioFileException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
						}
					});
		*/
			
		//	inputPanel.addPropertyChangeListener(propertyName, listener);
		
			/*
			inputPanel.addPropertyChangeListener("mixer", 
				new PropertyChangeListener() {
					@Override
					public void propertyChange(PropertyChangeEvent arg0) {
						try {
							
							setNewMixer());
							//System.out.println("###!!"  + arg0);
							//com.sun.media.sound.DirectAudioDevice@74b21e6a
						} catch (LineUnavailableException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						} catch (UnsupportedAudioFileException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
					}
				});
		
			*/
			
			
			//---------------
			if(dispatcher!= null){
				dispatcher.stop();
			}
			int bufferSize = 512;
			int overlap = 0;

			
			
			final AudioFormat format = new AudioFormat(44100f, 16, 1, true,
					true);
			final DataLine.Info dataLineInfo = new DataLine.Info(
					TargetDataLine.class, format);
			TargetDataLine line;
			line = (TargetDataLine) nv.getLine(dataLineInfo);
			final int numberOfSamples = bufferSize;
			//line.open(format, numberOfSamples);
			line.open(format, numberOfSamples);
			line.start();
			
			final AudioInputStream stream = new AudioInputStream(line);
			JVMAudioInputStream audioStream = new JVMAudioInputStream(stream);
			// create a new dispatcher
			dispatcher = new AudioDispatcher(audioStream, bufferSize,
					overlap);

			// add a processor, handle percussion event.
			silenceDetector = new SilenceDetector(threshold,false);
			dispatcher.addAudioProcessor(silenceDetector);
			dispatcher.addAudioProcessor(this);

			// run the dispatcher (on a new thread).
			new Thread(dispatcher,"Audio dispatching").start();
			
			//---------------
			
		//JSlider thresholdSlider = initialzeThresholdSlider();		
		//JPanel params = new JPanel(new BorderLayout());
		
		//params.setBorder(new TitledBorder("2. Set the algorithm parameters"));
		
		JLabel label = new JLabel("");
		label.setToolTipText("Energy level when sound is counted (dB SPL).");
	//	params.add(label,BorderLayout.NORTH);
		//params.add(thresholdSlider,BorderLayout.CENTER);
		
		JPanel inputAndParamsPanel = new JPanel(new BorderLayout());
		inputAndParamsPanel.add(inputPanel,BorderLayout.NORTH);
		//inputAndParamsPanel.add(params,BorderLayout.SOUTH);

		
		JPanel panelWithTextArea = new JPanel(new BorderLayout());
		textArea = new JTextArea(8,30);
		textArea.setEditable(false);
		panelWithTextArea.add(inputAndParamsPanel,BorderLayout.NORTH);
		panelWithTextArea.add(new JScrollPane(textArea),BorderLayout.CENTER);

		add(panelWithTextArea,BorderLayout.NORTH);
		
	
		graphPanel = new GaphPanel(threshold);
		graphPanel.setSize(80,100);
		add(graphPanel,BorderLayout.CENTER);
	}

	
	
	
	
	private static class GaphPanel extends JPanel{

		/**
		 * 
		 */
		private static final long serialVersionUID = 5969781241442094359L;
		private double threshold;
		private double maxLevel = -1000;
		private long currentModulo = System.currentTimeMillis()/15000;
		private List<Double> levels;
		private List<Long> startTimes;
		
		public GaphPanel(double defaultThreshold){			
			setThresholdLevel(defaultThreshold);
			levels = new ArrayList<Double>();
			startTimes=new ArrayList<Long>();
			setMinimumSize(new Dimension(80,60)); 
		}
		
		private void setMaxLevel(double newMaxLevel){
			if(newMaxLevel> maxLevel){
				maxLevel=newMaxLevel;
			}
		}
		
		public void setThresholdLevel(double newThreshold){
			threshold=newThreshold;
			repaint();
		}
		
		public void addDataPoint(double level,long ms){
			levels.add(level);
			startTimes.add(ms);
			setMaxLevel(level);
			repaint();
		}
		
		public void paintComponent(Graphics g) {
	        super.paintComponent(g); //paint background
	        g.setColor(Color.BLACK);
			g.fillRect(0, 0,getWidth(), getHeight());
			
			if(System.currentTimeMillis()/15000 > currentModulo){
				currentModulo = System.currentTimeMillis()/15000;
				levels.clear();
				startTimes.clear();
			}
			
			//draws the voice levels 
			
			for(int i =0 ; i < levels.size();i++){
				g.setColor( levels.get(i) > threshold ? Color.GREEN:Color.ORANGE ); 
				int x = msToXCoordinate(startTimes.get(i));
				int y = levelToYCoordinate(levels.get(i));
				g.drawLine(x, y, x+1, y);
			}
			
			int thresholdYCoordinate = levelToYCoordinate(threshold);
			g.setColor(Color.ORANGE);
			g.drawLine(0, thresholdYCoordinate, getWidth(),thresholdYCoordinate);
			g.drawString(String.valueOf((int)threshold), 0, thresholdYCoordinate + 15);
			
			
			int maxYCoordinate = levelToYCoordinate(maxLevel);
			g.setColor(Color.RED);
			g.drawLine(0, maxYCoordinate, getWidth(),maxYCoordinate);
			g.drawString(String.valueOf(((int)(maxLevel*100))/100.0), getWidth() - 40, maxYCoordinate + 15);
			
	    }
		
		private int levelToYCoordinate(double level){
			int inPixels = (int)((120 + level)  / 120 * (getHeight()-1));
			int yCoordinate =  getHeight() - inPixels;
			return yCoordinate;
		}
		
		private int msToXCoordinate(long ms){
			return (int) ((ms % 15000)/15000.0 * getWidth());
		}
				
	}
	
	
/*
	private JSlider initialzeThresholdSlider() {
		JSlider thresholdSlider = new JSlider(0,0);
		//thresholdSlider.setValue((int)threshold);
		thresholdSlider.setPaintLabels(false); //numbers
		thresholdSlider.setPaintTicks(false); //ticks
		//thresholdSlider.setMajorTickSpacing(20);
		//thresholdSlider.setMinorTickSpacing(10);
		/*
		thresholdSlider.addChangeListener(new ChangeListener(){
			@Override
			public void stateChanged(ChangeEvent e) {
				
				JSlider source = (JSlider) e.getSource();
				threshold = source.getValue();
				
				//tbd = threshold; 
				graphPanel.setThresholdLevel(threshold);
			    if (!source.getValueIsAdjusting()) {			        
			        try {
						setNewMixer(currentMixer);
					} catch (LineUnavailableException e1) {
						// TODO Auto-generated catch block
						e1.printStackTrace();
					} catch (UnsupportedAudioFileException e1) {
						// TODO Auto-generated catch block
						e1.printStackTrace();
					}
			    }
			}
		});*/ 
		
		//return thresholdSlider;
	//}
	

	private void setNewMixer() throws LineUnavailableException,
			UnsupportedAudioFileException {
		
		if(dispatcher!= null){
			dispatcher.stop();
		}
		//currentMixer = mixer;
		
		
		float sampleRate = 44100;
		int bufferSize = 512;
		int overlap = 0;
	
		/*
		final AudioFormat format00 = new AudioFormat(44100f, 16, 1, true,true);
		TargetDataLine tdl = AudioSystem.getTargetDataLine(format00);
		final DataLine.Info infoIN = new DataLine.Info(TargetDataLine.class, format00);
		tdl = (TargetDataLine) nv.getLine(infoIN); //AudioSystem.getLine(infoIN);
		tdl.open(format00);
		//tdl.start();
		new Thread(dispatcher,"Audio dispatching").start();
		*/
		
		textArea.append("Started listening with " + Shared.toLocalString(nv.getMixerInfo().getName()) + "\n\tparams: " + threshold + "dB\n");
		
		if(nv.getMixerInfo().getName().equals("Primary Sound Capture Driver")){
			System.out.println("dsjhfalkdjhflkjahsdlkj");
		}
		
		
		final AudioFormat format = new AudioFormat(sampleRate, 16, 1, true,
				true);
		final DataLine.Info dataLineInfo = new DataLine.Info(
				TargetDataLine.class, format);
		TargetDataLine line;
		line = (TargetDataLine) nv.getLine(dataLineInfo);
		final int numberOfSamples = bufferSize;
		//line.open(format, numberOfSamples);
		line.open(format, numberOfSamples);
		line.start();
		final AudioInputStream stream = new AudioInputStream(line);

		JVMAudioInputStream audioStream = new JVMAudioInputStream(stream);
		// create a new dispatcher
		dispatcher = new AudioDispatcher(audioStream, bufferSize,
				overlap);

		// add a processor, handle percussion event.
		silenceDetector = new SilenceDetector(threshold,false);
		dispatcher.addAudioProcessor(silenceDetector);
		dispatcher.addAudioProcessor(this);

		// run the dispatcher (on a new thread).
		new Thread(dispatcher,"Audio dispatching").start();
		//*/ 
	}

	public static void main(String... strings) throws LineUnavailableException {
		JFrame frame = new SoundDetector();
		frame.pack();
		frame.setSize(640,480);
		frame.setVisible(true);//throws InterruptedException,
	}
	/*
			InvocationTargetException {
		SwingUtilities.invokeAndWait(new Runnable() {
			@Override
			public void run() {
				JFrame frame = new SoundDetector();
				frame.pack();
				frame.setSize(640,480);
				frame.setVisible(true);
			}
		});
	}*/ 

	@Override
	public boolean process(AudioEvent audioEvent) {
		handleSound();
		return true;
	}

	
	//important function - handles what happens when sound is above or below a threshold and what is printed (for testing) 
	private void handleSound(){
	 
		if(silenceDetector.currentSPL() > threshold){
			//textArea.append("Sound detected at:" + System.currentTimeMillis() + ", " + (int)(silenceDetector.currentSPL()) + "dB SPL\n");
			textArea.append("Decibel level:" + (int)(silenceDetector.currentSPL()) + "dB SPL\n");
			textArea.setCaretPosition(textArea.getDocument().getLength());
			
			//gets the average of the users volume 
			avg = ((int)silenceDetector.currentSPL()*-1) + avg;  
			cnt++; 
			pnt = avg/cnt;			
			
			//System.out.println("levl===" + (int)silenceDetector.currentSPL());
			System.out.println("testing avg " + pnt);
		}
		graphPanel.addDataPoint(silenceDetector.currentSPL(), System.currentTimeMillis()); //draws graph		
		
		//test if the sound level goes above a threshhold 
		if((int)(silenceDetector.currentSPL()) > -70){
			System.out.println("Went above -70");
			//System.out.println("THRESHOLD========" + tbd);
		}
		
	}
	
	
	@Override
	public void processingFinished() {		
		
	}
}
