# KeyOtp
KeyOtp is a small hidden application that allows you to quickly pull a timed one time password into you clipboard without your hands leaving your keyboard.

### Installation
Simple, double click the install, next -> next -> next etc.
Then run KeyOtp.exe from windows start menu (haven't figured out how to make it auto start after install).


### Usage
Press the key combo: SHIFT + WIN + O, and a window will pop up.
If you don't have any Totp keys defined, click the settings button at the bottom of the window:
![image](https://user-images.githubusercontent.com/6437746/190589599-b24b5fcc-119a-419e-b4ad-88cdb6ff1587.png)

Here you will be able to add/remove and edit any keys that you may need.
When editing a key you can enter any name you'd like and type in the secret key for the Totp, alternatively you can (try) and copy the QR code to your clipboard and hit the "from clipboard" button, this will (again... try) to parse the QR code's secret and place it in the field.
![image](https://user-images.githubusercontent.com/6437746/190590225-156a2efd-ed54-4074-adf5-b583426c676a.png)

NOTE: For now, you must click the "UPDATE" button when changing an invidiual key, **FOLLOWED BY** the **SAVE** button.
