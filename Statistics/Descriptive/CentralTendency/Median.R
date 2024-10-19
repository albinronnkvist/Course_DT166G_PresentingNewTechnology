# Odd number of items in a skewed data set
data_odd <- c(10, 12, 14, 15, 100)

odd_median_value <- median(data_odd)
odd_mean_value <- mean(data_odd)

# The median gives a more accurate result than the mean when the data is skewed
odd_median_value # 14
odd_mean_value # 30.2



# Even number of items in data set
data_even <- c(4, 8, 6, 5, 3, 7)

even_median_value <- median(data_even)

even_median_value