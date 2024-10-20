hours_studied <- c(2, 4, 6, 8, 10, 12, 14, 16, 18, 20)
exam_scores <- c(50, 55, 60, 65, 67, 75, 80, 81, 90, 95)

## Shapiro-Wilk normality test
hours_studied_normality <- shapiro.test(hours_studied)
exam_scores_normality <- shapiro.test(exam_scores)

## Scatter plot to check linearity and variance
plot(hours_studied, exam_scores, main = "Scatter plot of Hours Studied vs. Exam Scores",
     xlab = "Hours Studied", ylab = "Exam Scores")

## Pearson correlation test
correlation_test <- cor.test(hours_studied, exam_scores, method = "pearson")

## Print results
print(hours_studied_normality)
print(exam_scores_normality)
cat("Correlation coefficient:", correlation_test$estimate, "\n")
cat("p-value:", correlation_test$p.value, "\n")