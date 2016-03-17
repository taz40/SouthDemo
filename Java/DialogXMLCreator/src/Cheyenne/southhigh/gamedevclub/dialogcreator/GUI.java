package Cheyenne.southhigh.gamedevclub.dialogcreator;

import java.awt.BorderLayout;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.util.ArrayList;
import java.util.Stack;

import javax.swing.JButton;
import javax.swing.JFileChooser;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.UIManager;
import javax.swing.UnsupportedLookAndFeelException;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;

import org.w3c.dom.Document;
import org.w3c.dom.NamedNodeMap;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

public class GUI extends JFrame implements ActionListener {

	private JPanel contentPane;
	int windowHeight = 800;
	int windowWidth = 600;
	int state = 0;
	ArrayList<Choice> choices = new ArrayList<Choice>();
	Stack<Choice> choiceStack = new Stack<Choice>();
	Stack<Option> optionStack = new Stack<Option>();
	Choice currentChoice;
	Option currentOption;
	String path;
	
	public void loadXMLFile(String filename){
		try{
			DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
			DocumentBuilder db = dbf.newDocumentBuilder();
			Document doc = db.parse(new File(filename));
			NodeList nodeList = doc.getChildNodes();
			doNodes(nodeList);
		}catch (Exception e){
			e.printStackTrace();
		}
		
	}
	
	void doNodes(NodeList nodes){
		for(int i = 0; i < nodes.getLength(); i++){
			Node node = nodes.item(i);
			if(node.getNodeName().equals("Choice")){
				if(currentChoice != null)
					choiceStack.push(currentChoice);
				currentChoice = new Choice();
				NamedNodeMap attributes = node.getAttributes();
				currentChoice.Description = attributes.getNamedItem("Desc").getNodeValue();
				currentChoice.name = attributes.getNamedItem("id").getNodeValue();
				currentChoice.tag = attributes.getNamedItem("tag").getNodeValue();
				doNodes(node.getChildNodes());
				if(currentOption != null){
					currentOption.resultingChoice = currentChoice;
				}else{
					choices.add(currentChoice);
				}
				if(choiceStack.isEmpty()){
					currentChoice = null;
				}else{
					currentChoice = choiceStack.pop();
				}
			}else if(node.getNodeName().equals("Option")){
				if(currentOption != null)
					optionStack.push(currentOption);
				currentOption = new Option();
				NamedNodeMap attributes = node.getAttributes();
				currentOption.Description = attributes.getNamedItem("Desc").getNodeValue();
				currentOption.Response = attributes.getNamedItem("Response").getNodeValue();
				doNodes(node.getChildNodes());
				if(currentChoice != null)
					currentChoice.Options.add(currentOption);
				if(optionStack.isEmpty())
					currentOption = null;
				else
					currentOption = optionStack.pop();
			}else if(node.getNodeName().equals("Choices")){
				doNodes(node.getChildNodes());
			}
		}
	}

	/**
	 * Launch the application.
	 */
//	public static void main(String[] args) {
//		EventQueue.invokeLater(new Runnable() {
//			public void run() {
//				try {
//					MachineSelection frame = new MachineSelection();
//					frame.setVisible(true);
//				} catch (Exception e) {
//					e.printStackTrace();
//				}
//			}
//		});
//	}

	/**
	 * Create the frame.
	 */
	
	public void setupButtons(){
		setSize(600, 800);
			
	     setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	      // Set layout for the whole dialog
	      JPanel contentPane = (JPanel)this.getContentPane();
	      contentPane.setLayout(new BorderLayout());
	      
	      // Set layout for contactListPane
	      JPanel contactListPanel = new JPanel();
	      contactListPanel.setLayout(new GridLayout(15, 1)); // 15 rows, 1 column
	      
	      //(...)
	      
	      //Add a button for each contact in the address book (from a LinkedList)
	      if(state == 0){
	    	  System.out.println(choices.size());
	    	  for(int i = 0; i < choices.size(); i++){
	    		  Choice c = choices.get(i);
	    		  JButton button = new JButton();
			      button.setText(c.name); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
			      contactListPanel.add(button);
			      button.addActionListener(this);
			      button.setActionCommand(c.name);
	    	  }
	      }
	      
	      //(...)
	      
	      JScrollPane scrollPane = new JScrollPane(contactListPanel);
	      // Add the contactListPane to the main content area (inside scrollPane)
	      contentPane.add(scrollPane, BorderLayout.CENTER);
	      setVisible(true);
	}
	
	public GUI(boolean newFile) {
		JFileChooser fileChooser = new JFileChooser();
		fileChooser.setFileSelectionMode(JFileChooser.FILES_ONLY);
		fileChooser.setFileFilter(new XMLFileFilter());
		if(newFile){
			fileChooser.showSaveDialog(null);
			path = fileChooser.getSelectedFile().toString();
		}else{
			fileChooser.showOpenDialog(null);
			path = fileChooser.getSelectedFile().toString();
			loadXMLFile(path);
		}
		 try {
             UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
         } catch (ClassNotFoundException | InstantiationException | IllegalAccessException | UnsupportedLookAndFeelException ex) {
             ex.printStackTrace();
         }
		this.setTitle("Dialog Management");
		setupButtons();
	}

	@Override
	public void actionPerformed(ActionEvent event) {
		// TODO Auto-generated method stub
		if(state == 0){
			
		}
	}

}
