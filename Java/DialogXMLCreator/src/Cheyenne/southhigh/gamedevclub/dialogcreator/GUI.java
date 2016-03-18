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
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.UIManager;
import javax.swing.UnsupportedLookAndFeelException;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.Result;
import javax.xml.transform.Source;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.NamedNodeMap;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

public class GUI extends JFrame implements ActionListener {

	private JPanel contentPane;
	int windowHeight = 800;
	int windowWidth = 600;
	int state = 0; // 0 = choiceGroupSelector, 1 = ChoiceSelector, 2 = ChoiceInfo, 3 = OptionInfo
	ArrayList<ChoiceGroup> choiceGroups = new ArrayList<ChoiceGroup>();
	Stack<Choice> choiceStack = new Stack<Choice>();
	Stack<Option> optionStack = new Stack<Option>();
	ChoiceGroup currentGroup;
	Choice currentChoice;
	Option currentOption;
	String path;
	
	public void saveXMLFile(){
		try{
			DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
			DocumentBuilder db = dbf.newDocumentBuilder();
			Document doc = db.newDocument();
			Element root = doc.createElement("Choices");
			for(ChoiceGroup g : choiceGroups){
				Element group = doc.createElement("ChoiceGroup");
				group.setAttribute("name", g.name);
				for(Choice c : g.choices){
					addChoice(c, group, doc);
				}
				root.appendChild(group);
			}
			doc.appendChild(root);
			Transformer transformer = TransformerFactory.newInstance().newTransformer();
			Result output = new StreamResult(new File(path));
			Source input = new DOMSource(doc);

			transformer.transform(input, output);
		}catch (Exception e){
			e.printStackTrace();
		}
		
	}
	
	public void addChoice(Choice c, Element group, Document doc){
		Element choice = doc.createElement("Choice");
		choice.setAttribute("id", c.name);
		choice.setAttribute("Desc", c.Description);
		for(Option o : c.Options){
			addOption(o, choice, doc);
		}
		group.appendChild(choice);
	}
	
	public void addOption(Option o, Element choice, Document doc){
		Element option = doc.createElement("Option");
		option.setAttribute("Desc", o.Description);
		option.setAttribute("Response", o.Response);
		if(o.resultingChoice != null){
			addChoice(o.resultingChoice, option, doc);
		}
		choice.appendChild(option);
	}
	
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
				doNodes(node.getChildNodes());
				if(currentOption != null){
					currentOption.resultingChoice = currentChoice;
				}else{
					currentGroup.choices.add(currentChoice);
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
			}else if(node.getNodeName().equals("ChoiceGroup")){
				currentGroup = new ChoiceGroup();
				NamedNodeMap attributes = node.getAttributes();
				currentGroup.name = attributes.getNamedItem("name").getNodeValue();
				doNodes(node.getChildNodes());
				choiceGroups.add(currentGroup);
				currentGroup = null;
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
	    	  for(int i = 0; i < choiceGroups.size(); i++){
	    		  ChoiceGroup c = choiceGroups.get(i);
	    		  JButton button = new JButton();
			      button.setText(c.name); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
			      contactListPanel.add(button);
			      button.addActionListener(this);
			      button.setActionCommand(c.name);
	    	  }
	    	  
	    	  JButton button = new JButton();
		      button.setText("Add Group"); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Add");
	      }else if(state == 1){
	    	  JButton button = new JButton();
		      button.setText("Name: " + currentGroup.name); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Name");
		      
		      button = new JButton();
		      button.setText("Remove Group(and all choices within)"); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Remove");
		      
		      JLabel label = new JLabel("Choices:");
		      contactListPanel.add(label);
		      
	    	  for(int i = 0; i < currentGroup.choices.size(); i++){
	    		  Choice c = currentGroup.choices.get(i);
	    		  button = new JButton();
			      button.setText(c.name); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
			      contactListPanel.add(button);
			      button.addActionListener(this);
			      button.setActionCommand(c.name);
	    	  }
	    	  button = new JButton();
		      button.setText("Add Choice"); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Add");
	    	  
    		  button = new JButton();
		      button.setText("Back"); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Back");
	      }else if(state == 2){
	    	  JButton button = new JButton();
		      button.setText("Name: " + currentChoice.name); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Name");
		      
		      button = new JButton();
		      button.setText("Description: " + currentChoice.Description); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Desc");
		      
		      button = new JButton();
		      button.setText("Remove Choice"); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Remove");
		      
		      JLabel label = new JLabel("Options:");
		      contactListPanel.add(label);
		      
	    	  for(int i = 0; i < currentChoice.Options.size(); i++){
	    		  Option o = currentChoice.Options.get(i);
	    		  button = new JButton();
			      button.setText(o.Description); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
			      contactListPanel.add(button);
			      button.addActionListener(this);
			      button.setActionCommand(o.Description);
	    	  }
	    	  
	    	  button = new JButton();
		      button.setText("Add Option"); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Add");
	    	  
    		  button = new JButton();
		      button.setText("Back"); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Back");
	      }else if(state == 3){
	    	  JButton button = new JButton();
		      button.setText("Name: " + currentOption.Description); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Name");
		      
		      button = new JButton();
		      button.setText("Response: " + currentOption.Response); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Response");
		      
		      button = new JButton();
		      button.setText("Remove Option"); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Remove");
		      
		      JLabel label = new JLabel("Resulting Choice:");
		      contactListPanel.add(label);
		      
		      if(currentOption.resultingChoice != null){
	    		  button = new JButton();
			      button.setText(currentOption.resultingChoice.name); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
			      contactListPanel.add(button);
			      button.addActionListener(this);
			      button.setActionCommand("resultingChoice");
		      }else{
		    	  button = new JButton();
			      button.setText("Add Resulting Choice"); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
			      contactListPanel.add(button);
			      button.addActionListener(this);
			      button.setActionCommand("Add");
		      }
			      
    		  button = new JButton();
		      button.setText("Back"); //contactList.get(i).getSurname() + ", " + contactList.get(i).getGivenName());
		      contactListPanel.add(button);
		      button.addActionListener(this);
		      button.setActionCommand("Back");
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
			if (!path .endsWith(".xml"))
				path += ".xml";
		}else{
			fileChooser.showOpenDialog(null);
			path = fileChooser.getSelectedFile().toString();
			if (!path .endsWith(".xml"))
				path += ".xml";
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
			if(event.getActionCommand().equals("Add")){
				String s = (String)JOptionPane.showInputDialog(
                        this,
                        "New Value:",
                        "ModifyValue",
                        JOptionPane.PLAIN_MESSAGE,
                        null,
                        null,
                        "Group");
				if(s == null)
					s = "";
				ChoiceGroup g = new ChoiceGroup();
				g.name = s;
				choiceGroups.add(g);
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}
			for(ChoiceGroup g : choiceGroups){
				if(g.name.equals(event.getActionCommand())){
					currentGroup = g;
					state = 1;
					this.setContentPane(new JPanel());
					setupButtons();
					break;
				}
			}
		}else if(state == 1){
			if(event.getActionCommand().equals("Name")){
				String s = (String)JOptionPane.showInputDialog(
                        this,
                        "New Value:",
                        "ModifyValue",
                        JOptionPane.PLAIN_MESSAGE,
                        null,
                        null,
                        currentGroup.name);
				if(s == null)
					s = "";
				currentGroup.name = s;
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}
			if(event.getActionCommand().equals("Remove")){
				choiceGroups.remove(currentGroup);
				currentGroup = null;
				state = 0;
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}else{
				for(Choice c : currentGroup.choices){
					if(c.name.equals(event.getActionCommand())){
						currentChoice = c;
						state = 2;
						this.setContentPane(new JPanel());
						setupButtons();
						break;
					}
				}
			}
			if(event.getActionCommand().equals("Add")){
				String s = (String)JOptionPane.showInputDialog(
                        this,
                        "New Value:",
                        "ModifyValue",
                        JOptionPane.PLAIN_MESSAGE,
                        null,
                        null,
                        "Choice");
				if(s == null)
					s = "";
				Choice c = new Choice();
				c.name = s;
				currentGroup.choices.add(c);
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}
			if(event.getActionCommand().equals("Back")){
				state = 0;
				this.setContentPane(new JPanel());
				setupButtons();
			}
		}else if(state == 2){
			if(event.getActionCommand().equals("Remove")){
				if(optionStack.isEmpty()){
					state = 1;
					currentGroup.choices.remove(currentChoice);
					currentChoice = null;
				}else{
					currentOption = optionStack.pop();
					state = 3;
					currentOption.resultingChoice = null;
					currentChoice = null;
				}
				setupButtons();
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}else{
				for(Option o : currentChoice.Options){
					if(o.Description.equals(event.getActionCommand())){
						currentOption = o;
						state = 3;
						choiceStack.push(currentChoice);
						this.setContentPane(new JPanel());
						setupButtons();
						break;
					}
				}
			}
			if(event.getActionCommand().equals("Add")){
				String s = (String)JOptionPane.showInputDialog(
                        this,
                        "New Value:",
                        "ModifyValue",
                        JOptionPane.PLAIN_MESSAGE,
                        null,
                        null,
                        "Choice");
				if(s == null)
					s = "";
				Option o = new Option();
				o.Description = s;
				currentChoice.Options.add(o);
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}
			if(event.getActionCommand().equals("Name")){
				String s = (String)JOptionPane.showInputDialog(
                        this,
                        "New Value:",
                        "ModifyValue",
                        JOptionPane.PLAIN_MESSAGE,
                        null,
                        null,
                        currentChoice.name);
				if(s == null)
					s = "";
				currentChoice.name = s;
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}
			if(event.getActionCommand().equals("Desc")){
				String s = (String)JOptionPane.showInputDialog(
                        this,
                        "New Value:",
                        "ModifyValue",
                        JOptionPane.PLAIN_MESSAGE,
                        null,
                        null,
                        currentChoice.Description);
				if(s == null)
					s = "";
				currentChoice.Description = s;
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}
			if(event.getActionCommand().equals("Back")){
				if(optionStack.isEmpty()){
					state = 1;
				}else{
					currentOption = optionStack.pop();
					state = 3;
				}
				this.setContentPane(new JPanel());
				setupButtons();
			}
		}else if(state == 3){
			if(event.getActionCommand().equals("Remove")){
				currentChoice = choiceStack.pop();
				currentChoice.Options.remove(currentOption);
				currentOption = null;
				state = 2;
				setupButtons();
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}
			if(event.getActionCommand().equals("Add")){
				String s = (String)JOptionPane.showInputDialog(
                        this,
                        "New Value:",
                        "ModifyValue",
                        JOptionPane.PLAIN_MESSAGE,
                        null,
                        null,
                        "Choice");
				if(s == null)
					s = "";
				Choice c = new Choice();
				c.name = s;
				currentOption.resultingChoice = c;
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}
			if(event.getActionCommand().equals("Name")){
				String s = (String)JOptionPane.showInputDialog(
                        this,
                        "New Value:",
                        "ModifyValue",
                        JOptionPane.PLAIN_MESSAGE,
                        null,
                        null,
                        currentOption.Description);
				if(s == null)
					s = "";
				currentOption.Description = s;
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}
			if(event.getActionCommand().equals("Response")){
				String s = (String)JOptionPane.showInputDialog(
                        this,
                        "New Value:",
                        "ModifyValue",
                        JOptionPane.PLAIN_MESSAGE,
                        null,
                        null,
                        currentOption.Response);
				if(s == null)
					s = "";
				currentOption.Response = s;
				saveXMLFile();
				this.setContentPane(new JPanel());
				setupButtons();
			}
			if(event.getActionCommand().equals("Back")){
				currentChoice = choiceStack.pop();
				state = 2;
				this.setContentPane(new JPanel());
				setupButtons();
			}
			
			if(event.getActionCommand().equals("resultingChoice")){
				optionStack.push(currentOption);
				currentChoice = currentOption.resultingChoice;
				state = 2;
				this.setContentPane(new JPanel());
				setupButtons();
			}
		}
	}

}