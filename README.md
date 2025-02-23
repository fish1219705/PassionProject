# PassionProject

## 🍰 Healthy Dessert Content Management System (CMS)

### Overview
This Healthy Dessert Content Management System (CMS) is designed for dessert lovers who want to enjoy sweet treats without compromising their health. Our platform provides a collection of non-sugar, non-fat, dairy-free, and vegetarian dessert options, helping users make informed dietary choices.

#### 🎯 Who is this for?
Tommie – A health-conscious dessert lover who can browse desserts, check ingredient details, and read real customer reviews to ensure the treats meet her dietary preferences.
Peiyu (Admin) – Manages the desserts by updating ingredient information, adding new desserts, and moderating customer reviews to help others make informed choices.

### Features:
✅ Healthy Dessert Collection – Browse desserts with dietary labels like no sugar, dairy-free, keto, and vegetarian-friendly.
✅ Ingredient Tracking – View detailed ingredient lists, including health-conscious alternatives like almond flour and stevia.
✅ Real Customer Reviews – Users can read authentic reviews from others who have tried the desserts.
✅ Enhanced Instructions (Beyond MVP) – Each dessert's instructions now specify the exact quantity of each ingredient, ensuring clear measurements. Additionally, a "Change Ingredient Option" field allows customers to customize their dessert by substituting ingredients based on their preferences.
✅ Role-Based Access Control – 
Guest Users can browse desserts, ingredients, reviews and instructions but cannot modify content.
Admin Users (Peiyu & Team) can add, update, and delete desserts, ingredients, instructions, and moderate reviews.

### The system consists of four core tables:
Desserts 🍰 – Stores dessert names, descriptions, and health features (e.g., keto-friendly, no dairy, no sugar).
Ingredients 🥄 – Lists all ingredients, indicating which are healthy-friendly (e.g., almond flour, stevia, coconut milk).
Reviews ⭐ – Captures real customer feedback on each dessert (one-to-many relationship with Desserts).
Instructions 📜 – Step-by-step preparation guide, referencing both desserts and ingredients (many-to-one relationships).

Relationships:
Desserts ↔ Ingredients (Many-to-Many)
Desserts ↔ Reviews (One-to-Many)
Instructions ↔ Desserts (Many-to-One)
Instructions ↔ Ingredients (Many-to-One)
Users ↔ Reviews (One-to-Many) – Ensuring only registered users can submit reviews.

User Roles
🔹 Guest Users (Tommie & Others) – Can view desserts, ingredients, instructions, and reviews but cannot add, edit, or delete content.
🔹 Admin Users (Peiyu & Team) – Have full control to add, update, and delete desserts, ingredients, instructions, and reviews.

### Technologies Used
C# / ASP.NET Core MVC
Entity Framework Core
ASP.NET Built-in Database (or SQLite in-memory storage)
LINQ
Bootstrap (for styling, optional)
Authentication System (Identity Framework or Role-Based Authentication)

### Future Improvements
🔹 📸 Image Uploads for Reviews (Top Priority) – Allow customers to upload real photos of the desserts they purchased, giving future buyers a more authentic and trustworthy preview of the products. This feature will enhance transparency and improve user engagement.
🔹 ⭐ Review Enhancements – Implement a 5-star rating system alongside written reviews for more intuitive feedback and better decision-making.
🔹 🔍 Advanced Filtering – Enable users to filter desserts based on dietary preferences (e.g., "Only show keto desserts" or "No dairy options") for a personalized browsing experience.




