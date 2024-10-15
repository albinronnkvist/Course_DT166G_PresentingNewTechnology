# Basic
# Dataset
data <- c(4, 8, 6, 5, 3, 7)

# Calculate the mean
mean_value <- mean(data)

# Output the result
mean_value



## Handling missing values
# Example dataset with missing values
data_with_na <- c(4, 8, NA, 5, 3, 7)

# Calculate the mean, ignoring missing values
mean_value_with_na <- mean(data_with_na, na.rm = TRUE)

# Output the result
mean_value_with_na