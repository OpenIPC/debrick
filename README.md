![OpenIPC logo][logo]

## Debrick

Open-source software for Windows, which allows you to flash the camera (on Goke SoC gk7205v200/210/300) in a few clicks, without using the command line and any additional programs (like PuTTY, Tftpd64, python, etc.)

Originally сreated to simplify (automate) the flashing of cameras, but can also be used to read dump flash, restore bricked/passworded cameras and writing [OpenIPC](https://openipc.org/) binary files (similar [burn](https://github.com/OpenIPC/burn)).

### How to use 

![](connection.jpg)

Run the program, follow the [video](https://www.youtube.com/watch?v=WQcVlOOUAro&t=111s) instructions from the [telkam](https://t.me/telkamInfo) project:

![](form.jpg)

### Notes 

Don't forget to reboot your device after flashing.

After flashing the OpenIPC binary file, you may need to reboot the device several times, and maybe use PuTTY to install the correct partitions ("run setnor16m"), follow the guide from OpenIPC.

### Technical support and donations

Please **_[support our project](https://openipc.org/support-open-source)_** with donations or orders for development or maintenance. Thank you!

[logo]: https://openipc.org/assets/openipc-logo-black.svg
