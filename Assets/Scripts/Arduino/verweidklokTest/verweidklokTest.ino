#include <Servo.h>

int endAngle = 180;
int beginAngle = 60;

Servo myservo;

bool movingServo = false;

void setup() {
  Serial.begin(9600);
  myservo.attach(9);
}

void loop() {  
  if (Serial.read() == 'A' && !movingServo) {
    movingServo = true;
    beginServo();

    endServo();
  }

}

void beginServo() {
  for (int pos = beginAngle; pos <= endAngle; pos += 1) { // goes from 0 degrees to 180 degrees
    // in steps of 1 degree
    myservo.write(pos);              // tell servo to go to position in variable 'pos'
    delay(15);
  }
  myservo.detach(); //stop sending pulses so the servo doesn't try to move

  //delay(3000);
  delay(15UL * 1000UL);
}

void endServo() {
  myservo.attach(9);

  for (int pos = endAngle; pos >= beginAngle; pos -= 1) { // goes from 180 degrees to 0 degrees
    myservo.write(pos);              // tell servo to go to position in variable 'pos'
    delay(15);                       // waits 15ms for the servo to reach the position
  }
  movingServo = false;
}
