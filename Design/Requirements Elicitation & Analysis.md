# Requirements Workshopping

## Elicitation

### Epic 1: Personal User Page

The goal of this feature is to allow users to sign up for an account with our application, which will allow them to save everything that they will do on the application, including making a workout plan, meal plan, and saving their own data and preferences. Additionally, users will be able to track their own analytics by uploading data from their wearable devices, such as a smart watch. Furthermore, users will be able to save "preferences", such as listing the type of equipment you currently have, or whether or not you're able to go to a gym. Initially, this will be the most important feature, since we can’t begin adding key components of the other features without having this done. Namely, users can’t save their data without creating an account. Once we’ve accomplished this, however, this epic can afford to sit on the back burner for a bit.

+ **Scope**
    
    + This epic is meant to allow users to have their own page to keep track of their personal information and preferences. Other epics will involve making use of the user page, but they should primarily use it for the purposes of interacting with the user's information.

+ **Gaps in Knowledge**

    + Technical Domain Knowledge

        + We need to have a better understanding of how to work with wearable devices. None of us have any meaningful experience working with smart watches, so this feature will likely be saved for a later moment in the development timeline. In the meantime, it would be very beneficial if we could have at least one or two developers practicing this skill.

    + Business Domain Knowledge

        + We need to expand our knowledge on how to ensure users' data remains secure and protected. To expand on that, we also need to figure out best practices for informing user's on the data that will be collected from them while using our app.

### Epic 2: Workout Plan Creation

The goal of this epic is to allow a user to create a workout routine for themselves using a variety of features in our application. These features include filling out a form and auto generating a workout plan, selecting exercises from a list to add to their routines, and receiving AI recommendations to improve their workout plan. The form itself will take into consideration factors like the type of equipment the user has access to, their fitness level, and the specific muscles they plan to target. If a user has enough experience or is confident in their knowledge, they can instead use the aforementioned list to add exercises to their routine instead. Like the form, the list will allow users to filter their search using similar factors.

+ **Scope**
    
    + This epic is solely meant to allow users to create their own workout plans. This does involve interaction with the user page, although it will only be for the purposes of extracting user information, such as the type of equipment they have available, and for saving workout plans for the user.

+ **Gaps in Knowledge**

    + Technical Domain Knowledge

        + We still need to figure out the ins and outs of the APIs that will be used in this epic. This includes the exercise database API, as well as the AI API. Although we've looked around at some options and tested them, we still need more time to fully push these APIs to their limits.

    + Business Domain Knowledge

        + Adding onto the API issues, we also need to read up more on the terms and agreements of using these APIs. While most of the APIs we've seen have fairly short pages detailing their terms of use, others have multi-page documents with more links to other pages detailing their exact terms. Members of the team will need to look through these carefully when selecting APIs to make sure we're in full compliance with the API holders' terms and agreements.

### Epic 3: Meal Plan Creation

The goal of this epic is to allow a user to look at various common foods and create their own meal plans. Users will be able to lookup a food item, see the macros and nutritional information that goes along with that item, and add it to their meal plan if they choose to. Users will also be able to ask an AI to create a recipe for them as well. Using the food items a user selects, they will be able to give an AI a written prompt to create a recipe using the ingredient's they have listed.

+ **Scope**
    
    + This epic revolves around the idea of letting users create their own meal plans by using selected food items and AI generated recipes. 

+ **Gaps in Knowledge**

    + Technical Domain Knowledge

        + Similar to the workout plan creation epic, we'll need to look into exactly what we're capable of doing with the APIs we'll choose to work with.

        + Again, with the AI, we'll have to see what the AI API we choose to use will be capable of. We can't afford to have it go off script, so we'll need to test it thoroughly to see what works and what doesn't.

    + Business Domain Knowledge

        + Like before, we're going to have to look through the terms of service before fully investing in an API. Although we've got a good idea of the APIs we want to use, there is still more reading that could be done.

### Epic 4: User Motivation and Retention

The goal of this epic is to implement features that will keep the user wanting to continue exercising and, most importantly, return to our application to do so. Although this sounds very general, there are steps we can take to ensure they stick with us. An important feature will be to implement a sense of socialization and competition with others. To do this, we will include a feature that adds leaderboards and challenges. This will cover topics such as most steps taken, the most time spent exercising, and the highest number of calories burned. To keep track of this, we'll track scores by using reliable trackers, such as smart watches or other fitness tracking devices. If a user wants to get more specific, they can create challenges for others to compete with. Going along with this, we'll want to create a feature that will give users personal achievements to complete on their own. This way, we can also attract the attention of those who don't feel like they're ready to compete with others.

+ **Scope**

    + The main focus of this epic is to include features that will keep users competing with others and with themselves, in the hopes that they will continue using our app.

+ **Gaps in Knowledge**

    + Technical Domain Knowledge

        + Something that's come up is the idea of real time updating. We're going to have to figure out how to avoid issues that could come up when we've got multiple users inputting data and causing the leaderboards or challenge tracker to end up in a deadlock.

        + As mentioned, we're going to have to expand our knowledge of fitness tracking devices if we really want to be able to pull off most of these features. 

    + Business Domain Knowledge

        + It would be worthwhile to look into any alternatives to tracking fitness data to keep things fair and square. Obviously, trusting people to input their own data will likely result in people cheating and ruining the fun for everyone. To put it more simply, we need to look into all realistic methods we can use to prevent cheating.

## Analysis

### Top Priority Entities

#### **Users**

+ **Bound**: Personal information, fitness data, and progress. 

+ **Limitations**: User’s privacy — any login information and personal data must be stored properly. Additionally, we have to tell the user what kind of information we’re storing before we store it.

+ **Types**
    + Strings

        + First Name, Last Name, Email, Password, Fitness Level, Fitness Goals, Location

    + Integer

        + UserID, BiometricID, any future foreign keys, Workouts Completed

    + Date

        + Last Login

#### **Workout Plan**

+ **Bound**: Personalized to the user’s fitness goals and available equipment.

+ **Limitations**: Limited by available exercise data and the AI's capabilities to generate appropriate suggestions.

+ **Types**

    + Strings

        + Plan Name, Difficulty, Goal

    + Date

        + Start Date, End Date

    + Integer
        
        + Frequency, Sets, any future foreign keys

#### Exercise

+ **Bound**: Describes each exercise's details like muscle group and required equipment. 

+ **Limitations**: The app will only support exercises it can find in the database. 

+ **Types**

    + String

        + Body Part, Exercise Name, Equipment, Gif URL, Target, Secondary Muscles, Instructions

    + Integer

        + ExerciseID, API Exercise ID

### All Entities and Attributes

+ **Entities**: User, Workout Plan, Exercise, Meal Plan, Food, Biometric Data, Gym, Fitness Challenge, Leaderboard.


+ **Attributes**

    + **User**: ID (int), First Name (string), Last Name (string), Email (string), Password (string), Fitness Level (string), Fitness Goals (string), Last Login (Date), Location (string), Workouts Completed (int).

    + **Workout Plan**: ID (int), Name (string), Frequency (int), Difficulty (string), Goal (string), Start Date (Date), End Date (Date). 
    Exercise: ID (int), Body Part (string), Equipment (string), Gif URL (string), Name (string), Target (string), Secondary Muscles (string), Instructions (string).

    + **Meal Plan**: ID (int), Start Date (date), End Date (date), Frequency (string), Target Proteins (float), Target Carbs (float), Target Fat (float), Target Calories (float), Plan Name (string)

    + **Food**: ID (int), Food Name (string), Calories (float), Protein (float), Carbs (float), Fat (float), Serving Size (float)

    + **Gym**: ID (int), Name (string), Address (string), Phone Number (int), URL (string), Available Equipment (string), Pricing Info (int), Membership Options (string)

    + **Leaderboard**: ID (int), Rank (int), Score (float) , UserID (int)

    + **Fitness Challenge**: ID (int), Name (string), Description (string), Start Date (Date), End Date (date)

    + **Biometric Data**: ID (int), UserID (int), Steps (int), Calories Burned (int), Heart Rate (int), Active Minutes (float)