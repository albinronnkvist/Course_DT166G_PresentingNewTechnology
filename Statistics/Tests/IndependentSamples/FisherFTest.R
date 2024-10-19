# Example 1: Hard-coded data with similar variances
group1 <- c(10, 12, 14, 16, 18)
group2 <- c(9, 11, 13, 15, 17)

var.test(group1, group2)

# Example 2: Hard-coded data with different variances
group3 <- c(5, 10, 15, 20, 25)
group4 <- c(5, 6, 7, 8, 9)

var.test(group3, group4)