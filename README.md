# User Simulation

---
## Introduction
Sim is a C# application that ingests an XML file and performs tasks based on the provided XML. It is meant to resemble user actions on a system. The goal of this is to help facilitate training and education by providing a more realistic environment to practice. 

Each user action is scriptable using the task block in the XML configuration. Tasks are comprised of two components:

* Task configuration: three configuration tags which determine the name of the task (for error tracing and configuration), the number of times the task will repeat, and how long to pause between each action.
* Task actions: This can be as many actions as you want, but it is recommended that tasks are more granular. Actions are what Sim will execute as part of simulating the user. Sim runs through each action like a script executing one action then the next in sequential order. Keep in mind, Sim does not wait for one action to complete before starting another, which is why the `<pause>` and `<sleep>` tags exists.  

---	
## Settings

There are two general configuration options for Sim which it is recommended to use. 

`<delete>`: Options for this are `true` or `false`. This determines if the XML file will be deleted after it is read into memory. Deletion occurs before the first loop begins. This is a good option in cases where secrets will be disclosed in the XML, such as passwords. If this is not set, it will default to false and the file will not be removed. 

`<errordirectory>`: The path to the file where errors will be written out. Every error will produce a separate error file. This is to help in troubleshooting your XML. When an error occurs the file will be named the name of your task. If no task name is set to a randomly generated GUID. In the case an error director is not set, User Simulation will default to `C:\Users\Public\Documents\`. 

---
## Tasks

There are two part for tasks with a variety of different tasks that can be run. First, the settings for the task should be filled out. This will help in troubleshooting and will ensure the task functions as expected. 
(include skeletal outline. Don't say it's made up of two things, and not use the name, say "tasks are comprised of a config block and an action block")


### Configuration
The configuration section is wrapped in `<config></config>` tags

`<name>`: The of the task. This is not a mandatory setting, but it is recommended while developing so troubleshooting and finding a specific task later is easier. If this is not set, when an error occurs User Simulation will generate a random GUID for the error log.

`<loop>`: Sets the number of time the task will be repeated. If this is not set, User Simulation will only run through the task once. If you the task to repeat four times you would set it like this: `<loop>4</loop>`

`<pause>`: This is the time, in milliseconds, the application will wait between each action in a task. If this is not set, it will default to 5000 milliseconds (5 seconds).

### Actions
Actions are what User Simulation will do to simulate user actions. These should be wrapped in the `<actions></actions>` tags. It is recommended that tasks are made fairly granular for organization and troubleshooting. You can also add comment in this (or really any) section for more information about what is supposed to be occurring.  

`<setclipboard>`: This sets the value of the clipboard. Any text in this value will be set as the clipboard value. 

`<getclipboard>`: This is simply an alias for {CONTROL}+v sent to the special keyboard function. It will paste whatever is in the clipboard at that time. There is no value that needs to be put in the tag; if it is present, whatever is in the clipboard will be pasted when the tags are found in the actions.
 
`<plain>`: Character `a-z`, `A-Z`, and `0-9` and some special characters which will be typed out in order. Most text will be typed out as it appears between the tags. Two exceptions are `&` and `<` which should be typed `&amp;` and `&lt;`. 

```
<plain>abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890</plain>
<plain>`!@#$%^*()_+-,./>?':"[]\{}|';</plain>
<plain>&amp;&lt;</plain>
<plain>1234567890</plain>
<plain>"This sentence will be typed out just like this!"</plain>
```

`special`: Special keys are handled differently from the regular keyboard. Special keys need to be wrapped in `{ }` for both individual and combination key presses. Individual key presses should be separated by a space. For special key presses, use a plus sign and no spaces. 

The available specials keys are as follows:

* `{BACKSPACE}`
* `{TAB}`
* `{CLEAR}`
* `{ENTER}`
* `{CONTROL}`
* `{ALT}`
* `{PAUSE}`
* `{CAPS}`
* `{ESC}`
* `{PAGEUP}`
* `{PAGEDOWN}`
* `{END}`
* `{HOME}`
* `{ARROWLEFT}`
* `{ARROWUP}`
* `{ARROWRIGHT}`
* `{ARROWDOWN}`
* `{SELECT}`
* `{PRINT}`
* `{EXECUTE}`
* `{PRINTSCREEN}`
* `{INSERT}`
* `{DELETE}`
* `{HELP}`
* `{F1}`
* `{F2}`
* `{F3}`
* `{F4}`
* `{F5}`
* `{F6}`
* `{F7}`
* `{F8}`
* `{F9}`
* `{F10}`
* `{F11}`
* `{F12}`
* `{F13}`
* `{F14}`
* `{F15}`
* `{F16}`

This is the tag to use for a combination of a special key and plain text key. The plain text key should be in lower case because upper case would press the shift key as well. If you need the letter to be uppercase, you would add in the {SHIFT} key.
	
Here is an example where User Simulation will press and release the Control key, then press the Control Shift Escape keys and then release them:

`<special>{CONTROL} {CONTROL}+{SHIFT}+{ESC}</special>`

`<process>`: The program that will be run. This can be one of three options:
* The full path to an executable. 
`<process>C:\Windows\notepad.exe</process>`
* The full path to a file which will open with the default program. 
`<process>C:\Users\User1\Desktop\example.txt</process>`
* The URL to a website which will open with the default browser.
`<process>https://google.com</process>`

`<kill>`: Kill should either be set to `true` or should not be present, but if the setting is present User Simulation will kill the processes. This kills every process which that specific task started. This is another reason that it is a good idea to make tasks more granular. If this tag is not present, it will default to `false` and will not kill the processes that task started. 
`<kill>true</kill>`

`<powershell>`: PowerShell functionality uses UnmanagedPowerShell. This will run PowerShell commands or can be use to run a PowerShell script. 
`<powershell>Command</powershell>`
`<powershell>script.ps1</powershell>`
This does not open a PowerShell command window. If you want it to open as if the user is opening window to run commands, it would be best to open PowerShell with a `<process>` to run the commands in the command prompt. 

`<mount>`: This attempts to mount a drive using net.exe use. At this time, it will attempt to connect using the current token. 
`<mount>\\server\share\location</mount>`

`<sleep>`: This is slightly different than pause because it is run like an action. If you are doing something that requires something to load, you can put a sleep time in to give it time to complete before the next action starts. The pause time in the configuration is still used so this is extra time on top of it. The time for this is in milliseconds. 
`<sleep>5000</sleep>`

---
## Example

Examples can be found in the `XMLexamples` folder. 

---
## Limitations

At this time, Sim is limited to the above actions. Mouse clicks and changing windows (except through the keyboard) are not available. 

---
## Tips

* The pause time should be higher rather than lower. Lower times could cause problems with one action starting before the last one completes. For example, browsers take a couple seconds to open and load. This can cause problems if a search bar is not loaded when typing starts. 

`<pause>5000</pause>`

* If you are having trouble with timing for some of the key presses, try breaking it up a bit and putting them in different actions. You can also put a sleep in the action to allow time for one action to complete before starting the next. 
	
```
<special>{ENTER}</special>
<special>{CONTROL}+c</special>
<special>{ALT}+{TAB}</special>
```

* Killing processes can get a little wonky with the more processes that start. It is best to keep the number of processes limited in the task to avoid problems with opening too many things and leaving them open. 

---

## Acknowledgment 

Thanks to [Dwight Hohnstein](https://twitter.com/djhohnstein) for helping me learn with this project.

UnmanagedPowerShell was taken from Lee Christensen's [UnmanagePowerShell](https://github.com/leechristensen/UnmanagedPowerShell) project. 