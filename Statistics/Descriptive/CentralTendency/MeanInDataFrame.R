# Example data frame
df <- data.frame(
  age = c(21, 25, 30, 22, 27, NA),
  salary = c(3000, 4000, 5000, 6000, 7000, 8000)
)

# Calculate the mean of the 'salary' column
mean_salary <- mean(df$salary)

# Output the result
mean_salary

# Calculate the mean of the 'age' column, ignoring missing values
mean_age <- mean(df$age, na.rm = TRUE)

# Output the result
mean_age